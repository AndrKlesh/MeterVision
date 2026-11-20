part of 'login_cubit.dart';

class LoginState extends Equatable {
  const LoginState({
    this.login = '',
    this.password = '',

    this.status = LoginStatus.initial,

    this.isLoading = false,
    this.isSuccess = false,

    this.errorMessage,

    this.loginError,
    this.passwordError,
  });

  final String login;
  final String password;

  final LoginStatus status;

  final bool isLoading;
  final bool isSuccess;

  final String? errorMessage;

  final String? loginError;

  final String? passwordError;

  LoginState copyWith({
    String? login,
    String? password,

    LoginStatus? status,

    bool? isLoading,
    bool? isSuccess,

    String? errorMessage,

    String? loginError,
    String? passwordError,

    bool clearError = false,
  }) {
    return LoginState(
      login: login ?? this.login,
      password: password ?? this.password,

      status: status ?? this.status,

      isLoading: isLoading ?? this.isLoading,
      isSuccess: isSuccess ?? this.isSuccess,

      loginError: loginError,
      passwordError: passwordError,

      errorMessage: clearError ? null : (errorMessage ?? this.errorMessage),
    );
  }

  @override
  List<Object?> get props => [
    login,
    password,
    status,
    isLoading,
    isSuccess,
    errorMessage,
    loginError,
    passwordError,
  ];
}
