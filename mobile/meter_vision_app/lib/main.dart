import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:meter_vision_app/core/theme/app_theme.dart';
import 'package:meter_vision_app/data/datasources/auth_service.dart';
import 'package:meter_vision_app/domain/usecase/login/login_usecase.dart';
import 'package:meter_vision_app/presentation/views/login/login_view.dart';

import 'package:meter_vision_app/data/repository/authentication_repository.dart';

import 'package:meter_vision_app/domain/usecase/scan_result/confirm_reading_usecase.dart';
import 'package:meter_vision_app/domain/usecase/scan_result/send_photo_usecase.dart';
import 'package:meter_vision_app/data/datasources/meter_service.dart';

import 'package:meter_vision_app/domain/repository/credentials_repository.dart';

void main() {
  final meter = MeterService();
  final authRepository = AuthService();

  final loginUseCase = LoginUseCase(authRepository);

  runApp(
    MultiRepositoryProvider(
      providers: [
        RepositoryProvider.value(value: SendPhotoUseCase(meter)),
        RepositoryProvider.value(value: ConfirmReadingUseCase(meter)),
        RepositoryProvider<ICredentialsRepository>(
          create: (_) => CredentialsRepository(),
        ),
        RepositoryProvider<LoginUseCase>.value(value: loginUseCase),
      ],
      child: const MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(theme: appTheme, home: const LoginView());
  }
}
