abstract class IAuthService {
  Future<void> login(String login, String password);

  Future<void> logout();
}
