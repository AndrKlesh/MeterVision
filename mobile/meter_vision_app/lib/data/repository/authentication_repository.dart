import 'package:shared_preferences/shared_preferences.dart';

import 'package:meter_vision_app/domain/repository/credentials_repository.dart';

class CredentialsRepository implements ICredentialsRepository {
  static const _key = 'login_credentials';
  final _preferences = SharedPreferencesAsync();

  @override
  Future<Credentials?> read() async {
    final pair = await _preferences.getStringList(_key);
    if (pair == null || pair.length != 2) return null;
    return (login: pair.first, password: pair.last);
  }

  @override
  Future<void> save(String login, String password) =>
      _preferences.setStringList(_key, [login, password]);

  @override
  Future<void> clear() => _preferences.remove(_key);
}
