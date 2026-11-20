import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:meter_vision_app/domain/usecase/login/login_usecase.dart';
import 'package:meter_vision_app/presentation/cubit/login/login_status.dart';

import 'package:meter_vision_app/domain/repository/credentials_repository.dart';

part 'login_state.dart';

class LoginCubit extends Cubit<LoginState> {
  LoginCubit(this._loginUseCase, this._credentials) : super(const LoginState());

  /// Обновляет значение логина на [value] и сбрасывает ошибку
  /// [value] - новый текст email
  void loginChanged(String value) {
    emit(state.copyWith(login: value, loginError: null));
  }

  /// Обновляет значение пароля на [value] и сбрасывает ошибку
  /// [value] - новый текст пароля
  void passwordChanged(String value) {
    emit(state.copyWith(password: value, passwordError: null));
  }

  /// Метод запроса на аутентификацию пользователя
  Future<void> login({required String login, required String password}) async {
    if (!_validate()) {
      return;
    }

    if (state.status == LoginStatus.loading) return;

    emit(state.copyWith(status: LoginStatus.loading, clearError: true));

    try {
      await _loginUseCase.login(login: login, password: password);

      await _credentials.save(login, password);
      if (isClosed) return;
      emit(state.copyWith(status: LoginStatus.success));
    } catch (e) {
      if (isClosed) return;
      emit(
        state.copyWith(status: LoginStatus.error, errorMessage: e.toString()),
      );
    }
  }

  Future<void> restore() async {
    emit(state.copyWith(status: LoginStatus.loading));
    try {
      final saved = await _credentials.read();
      if (saved != null) {
        await _loginUseCase.login(login: saved.login, password: saved.password);
      }
      if (!isClosed) {
        emit(
          state.copyWith(
            status: saved == null ? LoginStatus.initial : LoginStatus.success,
          ),
        );
      }
    } catch (e) {
      if (!isClosed) {
        emit(
          state.copyWith(status: LoginStatus.error, errorMessage: e.toString()),
        );
      }
    }
  }

  bool _validate() {
    final loginError = _validateLogin(state.login);
    final passwordError = _validatePassword(state.password);

    emit(state.copyWith(loginError: loginError, passwordError: passwordError));

    return loginError == null && passwordError == null;
  }

  String? _validateLogin(String value) {
    if (value.isEmpty) {
      return AppStrings.errorEmptyEmail;
    }

    if (!_emailRegex.hasMatch(value)) {
      return AppStrings.errorInvalidEmail;
    }

    return null;
  }

  String? _validatePassword(String value) {
    if (value.isEmpty) {
      return AppStrings.passwordHint;
    }
    return null;
  }

  final LoginUseCase _loginUseCase;
  final ICredentialsRepository _credentials;
  final RegExp _emailRegex = RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$');
}
