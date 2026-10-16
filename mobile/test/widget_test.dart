import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:metervision/features/auth/presentation/login_screen.dart';

Future<void> pumpLoginScreen(WidgetTester tester) async {
  // Увеличиваем экран, чтобы все элементы формы были видны.
  tester.view.physicalSize = const Size(1000, 1400);
  tester.view.devicePixelRatio = 1;

  addTearDown(tester.view.resetPhysicalSize);
  addTearDown(tester.view.resetDevicePixelRatio);

  await tester.pumpWidget(
    const MaterialApp(
      home: LoginScreen(),
    ),
  );
}

void main() {
  group('LoginScreen', () {
    testWidgets('Отображает основные элементы авторизации', (
      WidgetTester tester,
    ) async {
      await pumpLoginScreen(tester);

      expect(find.byKey(const Key('loginField')), findsOneWidget);
      expect(find.byKey(const Key('passwordField')), findsOneWidget);
      expect(find.byKey(const Key('loginButton')), findsOneWidget);

      expect(find.text('Войти'), findsOneWidget);
    });

    testWidgets('Пустые поля показывают ошибки валидации', (
      WidgetTester tester,
    ) async {
      await pumpLoginScreen(tester);

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pumpAndSettle();

      // Текст может встречаться дважды:
      // как подсказка поля и как сообщение об ошибке.
      expect(find.text('Введите логин'), findsNWidgets(2));
      expect(find.text('Введите пароль'), findsNWidgets(2));
    });

    testWidgets('Переключает видимость пароля', (
      WidgetTester tester,
    ) async {
      await pumpLoginScreen(tester);

      final passwordField = find.byKey(const Key('passwordField'));
      final toggleButton = find.byKey(const Key('togglePassword'));

      expect(
        tester.widget<EditableText>(
          find.descendant(
            of: passwordField,
            matching: find.byType(EditableText),
          ),
        ).obscureText,
        isTrue,
      );

      await tester.tap(toggleButton);
      await tester.pumpAndSettle();

      expect(
        tester.widget<EditableText>(
          find.descendant(
            of: passwordField,
            matching: find.byType(EditableText),
          ),
        ).obscureText,
        isFalse,
      );
    });

    testWidgets('Показывает загрузку при попытке входа', (
      WidgetTester tester,
    ) async {
      await pumpLoginScreen(tester);

      await tester.enterText(
        find.byKey(const Key('loginField')),
        'testuser',
      );
      await tester.enterText(
        find.byKey(const Key('passwordField')),
        'password123',
      );

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pump();

      expect(find.byType(CircularProgressIndicator), findsOneWidget);


      await tester.pump(const Duration(seconds: 1));
      await tester.pumpAndSettle();

      expect(
        find.text('Не удалось войти. Проверьте логин и пароль.'),
        findsOneWidget,
      );
    });

    testWidgets('Не выполняет вход при коротких данных', (
      WidgetTester tester,
    ) async {
      await pumpLoginScreen(tester);

      await tester.enterText(
        find.byKey(const Key('loginField')),
        'ab',
      );
      await tester.enterText(
        find.byKey(const Key('passwordField')),
        '123',
      );

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pumpAndSettle();

      expect(find.text('Минимум 3 символа'), findsOneWidget);
      expect(find.text('Минимум 6 символов'), findsOneWidget);

      // Validacia
      expect(find.byType(CircularProgressIndicator), findsNothing);
    });
  });
}
