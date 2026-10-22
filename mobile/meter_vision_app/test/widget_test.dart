import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:meter_vision_app/presentation/views/login/login_view.dart';

void main() {
  Widget createApp() {
    return const MaterialApp(home: LoginView());
  }

  group('MeterVision — экран авторизации', () {
    testWidgets('Отображает поля логина и пароля', (tester) async {
      await tester.pumpWidget(createApp());

      expect(find.text('MeterVision'), findsOneWidget);
      expect(find.text('Вход в аккаунт'), findsOneWidget);
      expect(find.byKey(const Key('loginField')), findsOneWidget);
      expect(find.byKey(const Key('passwordField')), findsOneWidget);
      expect(find.byKey(const Key('loginButton')), findsOneWidget);
    });

    testWidgets('Пустые поля показывают ошибки валидации', (tester) async {
      await tester.pumpWidget(createApp());

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pump();

      expect(find.text('Введите логин').last, findsOneWidget);
      expect(find.text('Введите пароль').last, findsOneWidget);
    });
    testWidgets('Короткий логин показывает ошибку', (tester) async {
      await tester.pumpWidget(createApp());

      await tester.enterText(find.byKey(const Key('loginField')), 'ab');
      await tester.enterText(find.byKey(const Key('passwordField')), '123456');

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pump();

      expect(find.text('Минимум 3 символа'), findsOneWidget);
    });

    testWidgets('Короткий пароль показывает ошибку', (tester) async {
      await tester.pumpWidget(createApp());

      await tester.enterText(find.byKey(const Key('loginField')), 'alex');
      await tester.enterText(find.byKey(const Key('passwordField')), '123');

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pump();

      expect(find.text('Минимум 6 символов'), findsOneWidget);
    });

    testWidgets('Кнопка показа пароля меняет видимость', (tester) async {
      await tester.pumpWidget(createApp());

      expect(find.byIcon(Icons.visibility_outlined), findsOneWidget);
      expect(find.byIcon(Icons.visibility_off_outlined), findsNothing);

      await tester.tap(find.byKey(const Key('togglePassword')));
      await tester.pump();

      expect(find.byIcon(Icons.visibility_off_outlined), findsOneWidget);
      expect(find.byIcon(Icons.visibility_outlined), findsNothing);
    });

    testWidgets('После отправки валидных данных показывается ошибка входа', (
      tester,
    ) async {
      await tester.pumpWidget(createApp());

      await tester.enterText(find.byKey(const Key('loginField')), 'alex');
      await tester.enterText(find.byKey(const Key('passwordField')), '123456');

      await tester.tap(find.byKey(const Key('loginButton')));
      await tester.pump();

      expect(find.byType(CircularProgressIndicator), findsOneWidget);

      await tester.pump(const Duration(seconds: 1));
      await tester.pump();

      expect(
        find.text('Не удалось войти. Проверьте логин и пароль.'),
        findsOneWidget,
      );
    });
  });
}
