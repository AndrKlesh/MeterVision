import 'package:meter_vision_app/domain/datasources/auth/i_auth_api_service.dart';

class LoginUseCase {
  LoginUseCase(this._service);
  final IAuthService _service;

  Future<void> login({required String login, required String password}) =>
      _service.login(login, password);

  Future<void> logout() => _service.logout();
}
