typedef Credentials = ({String login, String password});

abstract interface class ICredentialsRepository {
  Future<Credentials?> read();
  Future<void> save(String login, String password);
  Future<void> clear();
}
