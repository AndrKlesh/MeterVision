import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/domain/datasources/auth/i_auth_api_service.dart';

class AuthService extends IAuthService {
  @override
  Future<void> login(String login, String password) async {
    await Future.delayed(Duration(seconds: 1));

    if (!_DemoAccountFactory.create().matches(login, password)) {
      throw Exception(AppStrings.invalidCredentials);
    }
  }

  @override
  Future<void> logout() async {
    await Future.delayed(Duration(seconds: 1));
  }
}

class _DemoAccountFactory {
  static _DemoAccount create() => const _DemoAccount('user@mail.ru', '1234');
}

class _DemoAccount {
  const _DemoAccount(this.email, this.password);
  final String email;
  final String password;

  bool matches(String login, String enteredPassword) =>
      login.trim() == email && enteredPassword == password;
}
