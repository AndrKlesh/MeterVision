import 'dart:async';

import 'package:camera_platform_interface/camera_platform_interface.dart'
show CameraPlatform;
import 'package:camera_platform_interface/src/method_channel/method_channel_camera.dart'
show MethodChannelCamera;
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/presentation/views/camera/camera_view.dart';

const MethodChannel _permissionsChannel = MethodChannel(
  'flutter.baseflow.com/permissions/methods',
);
const MethodChannel _cameraChannel = MethodChannel('plugins.flutter.io/camera');


const int _denied = 0;
const int _granted = 1;
const int _permanentlyDenied = 4;

int _settingsCalls = 0;
int _lastCameraId = 0;
final List<MethodCall> _cameraCalls = [];

TestDefaultBinaryMessenger get _messenger =>
    TestDefaultBinaryMessengerBinding.instance.defaultBinaryMessenger;


void _mockPermission(int status, {Completer<void>? gate}) {
  _messenger.setMockMethodCallHandler(_permissionsChannel, (call) async {
    if (call.method == 'requestPermissions') {
      if (gate != null) await gate.future;
      final permissions = call.arguments as List<dynamic>;
      return {for (final p in permissions) p: status};
    }
    if (call.method == 'openAppSettings') {
      _settingsCalls++;
      return true;
    }
    return null;
  });
}


Future<void> _sendCameraInitialized(int cameraId) async {
  final platform = CameraPlatform.instance as MethodChannelCamera;
  await platform.handleCameraMethodCall(
    const MethodCall('initialized', {
      'previewWidth': 1280.0,
      'previewHeight': 720.0,
      'exposureMode': 'auto',
      'focusMode': 'auto',
      'exposurePointSupported': true,
      'focusPointSupported': true,
    }),
    cameraId,
  );
}


void _mockCamera({bool noCameras = false, bool shotFails = false}) {
  _messenger.setMockMethodCallHandler(_cameraChannel, (call) async {
    _cameraCalls.add(call);
    switch (call.method) {
      case 'availableCameras':
        if (noCameras) return <Map<String, Object?>>[];
        return <Map<String, Object?>>[
          {'name': 'back', 'lensFacing': 'back', 'sensorOrientation': 90},
          {'name': 'front', 'lensFacing': 'front', 'sensorOrientation': 270},
        ];
      case 'create':
        return <String, Object?>{'cameraId': ++_lastCameraId};
      case 'initialize':
        final id = (call.arguments as Map)['cameraId'] as int;
        unawaited(_sendCameraInitialized(id));
        return null;
      case 'takePicture':
        if (shotFails) throw PlatformException(code: 'shot_failed');
        return '/tmp/photo.jpg';
      default:
        return null;
    }
  });
}

int _callsOf(String method) =>
    _cameraCalls.where((c) => c.method == method).length;

List<Object?> _flashModes() => _cameraCalls
    .where((c) => c.method == 'setFlashMode')
    .map((c) => (c.arguments as Map)['mode'])
    .toList();


Future<void> _pumpView(WidgetTester tester) async {
  await tester.pumpWidget(const MaterialApp(home: CameraView()));
  await tester.pumpAndSettle();
}


Future<void> _pumpFrames(WidgetTester tester) async {
  for (var i = 0; i < 6; i++) {
    await tester.pump(const Duration(milliseconds: 100));
  }
}


Future<void> _pumpReadyView(
  WidgetTester tester, {
  bool shotFails = false,
}) async {
  _mockPermission(_granted);
  _mockCamera(shotFails: shotFails);
  await tester.pumpWidget(const MaterialApp(home: CameraView()));

  for (var i = 0; i < 40; i++) {
    await tester.pump(const Duration(milliseconds: 50));
    if (find.byKey(const Key('shutter_button')).evaluate().isNotEmpty) break;
  }

  expect(
    find.byKey(const Key('shutter_button')),
    findsOneWidget,
    reason: 'Камера не перешла в состояние ready '
        '(вызовы камеры: ${_cameraCalls.map((c) => c.method).toList()})',
  );
}

void main() {
  setUp(() {
    _settingsCalls = 0;
    _lastCameraId = 0;
    _cameraCalls.clear();
  });

  tearDown(() {
    _messenger.setMockMethodCallHandler(_permissionsChannel, null);
    _messenger.setMockMethodCallHandler(_cameraChannel, null);
  });

  group('validatePhoto (ТЗ 5.1.2)', () {
    test('подходящий JPG проходит', () {
      expect(validatePhoto('a.jpg', 1000), isNull);
    });

    test('пустой снимок отклоняется', () {
      expect(validatePhoto('a.jpg', 0), AppStrings.errorPhotoEmpty);
    });

    test('неподдерживаемый формат отклоняется', () {
      expect(validatePhoto('a.gif', 1000), AppStrings.errorPhotoFormat);
    });

    test('слишком большой снимок отклоняется', () {
      expect(
        validatePhoto('a.jpg', 11 * 1024 * 1024),
        AppStrings.errorPhotoTooLarge,
      );
    });
  });

  group('CameraView: разрешения и состояния', () {
    testWidgets('пока идёт запрос разрешения, показывает индикатор',
        (tester) async {
      final gate = Completer<void>();
      _mockPermission(_denied, gate: gate);

      await tester.pumpWidget(const MaterialApp(home: CameraView()));
      await tester.pump();

      expect(find.byType(CircularProgressIndicator), findsOneWidget);

      gate.complete();
      await tester.pumpAndSettle();
    });

    testWidgets('нет доступа к камере: показывает кнопку «Разрешить»',
        (tester) async {
      _mockPermission(_denied);
      await _pumpView(tester);

      expect(find.text(AppStrings.cameraPermissionDenied), findsOneWidget);
      expect(find.text(AppStrings.cameraPermissionAllow), findsOneWidget);
    });

    testWidgets('доступ запрещён навсегда: кнопка открывает настройки',
        (tester) async {
      _mockPermission(_permanentlyDenied);
      await _pumpView(tester);

      expect(
        find.text(AppStrings.cameraPermissionPermanentlyDenied),
        findsOneWidget,
      );

      await tester.tap(find.byKey(const Key('permission_button')));
      await tester.pumpAndSettle();

      expect(_settingsCalls, 1);
    });

    testWidgets('камера недоступна: показывает сообщение и «Повторить»',
        (tester) async {
      _mockPermission(_granted);
      _mockCamera(noCameras: true);
      await _pumpView(tester);

      expect(find.text(AppStrings.cameraUnavailable), findsOneWidget);
      expect(find.text(AppStrings.cameraRetry), findsOneWidget);
    });
  });

  group('CameraView: готовая камера', () {
    testWidgets('показывает подсказку и все кнопки', (tester) async {
      await _pumpReadyView(tester);

      expect(find.text(AppStrings.cameraHint), findsOneWidget);
      expect(find.byKey(const Key('flash_button')), findsOneWidget);
      expect(find.byKey(const Key('menu_button')), findsOneWidget);
      expect(find.byKey(const Key('switch_button')), findsOneWidget);
      expect(find.byKey(const Key('shutter_button')), findsOneWidget);
      expect(find.byKey(const Key('gallery_button')), findsOneWidget);
    });

    testWidgets('фонарик включается и выключается', (tester) async {
      await _pumpReadyView(tester);


      await tester.tap(find.byKey(const Key('flash_button')));
      await _pumpFrames(tester);
      expect(_flashModes(), ['torch']);

      await tester.tap(find.byKey(const Key('flash_button')));
      await _pumpFrames(tester);
      expect(_flashModes(), ['torch', 'off']);
    });

    testWidgets('меню «три точки» открывает пункт «Выход»', (tester) async {
      await _pumpReadyView(tester);

      await tester.tap(find.byKey(const Key('menu_button')));
      await _pumpFrames(tester);
      expect(find.byKey(const Key('menu_exit')), findsOneWidget);
      expect(find.text(AppStrings.cameraMenuExit), findsOneWidget);

      await tester.tap(find.byKey(const Key('menu_exit')));
      await _pumpFrames(tester);
      expect(find.text(AppStrings.stubExitNotImplemented), findsOneWidget);
    });

    testWidgets('переключение камеры заново запускает камеру',
        (tester) async {
      await _pumpReadyView(tester);
      expect(_callsOf('create'), 1);

      await tester.tap(find.byKey(const Key('switch_button')));
      for (var i = 0; i < 20; i++) {
        await tester.pump(const Duration(milliseconds: 50));
      }

      expect(_callsOf('create'), 2);
      expect(_callsOf('dispose'), greaterThanOrEqualTo(1));
      expect(find.byKey(const Key('shutter_button')), findsOneWidget);
    });

    testWidgets('ошибка съёмки показывает шторку с сообщением',
        (tester) async {
      await _pumpReadyView(tester, shotFails: true);

      await tester.tap(find.byKey(const Key('shutter_button')));
      await _pumpFrames(tester);

      expect(find.text(AppStrings.errorTakePhoto), findsOneWidget);

      await tester.tap(find.text(AppStrings.cameraGotIt));
      await _pumpFrames(tester);
      expect(find.text(AppStrings.errorTakePhoto), findsNothing);
    });
  });
}