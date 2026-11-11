import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';
import 'package:meter_vision_app/domain/usecase/login/login_usecase.dart';
import 'package:meter_vision_app/presentation/cubit/login/login_cubit.dart';
import 'package:meter_vision_app/presentation/cubit/login/login_status.dart';
import 'package:meter_vision_app/presentation/views/camera/camera_view.dart';
import 'package:meter_vision_app/presentation/widgets/fields/labeled_text_field.dart';

import 'package:meter_vision_app/domain/repository/credentials_repository.dart';

class LoginView extends StatefulWidget {
  const LoginView({super.key});

  @override
  State<LoginView> createState() => _LoginViewState();
}

class _LoginViewState extends State<LoginView> {
  final _loginController = TextEditingController();
  final _passwordController = TextEditingController();

  bool _obscurePassword = true;

  @override
  void dispose() {
    _loginController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => LoginCubit(
        context.read<LoginUseCase>(),
        context.read<ICredentialsRepository>(),
      )..restore(),
      child: BlocListener<LoginCubit, LoginState>(
        listener: (context, state) {
          if (!context.mounted) return;
          if (state.status == LoginStatus.success) {
            Navigator.of(context).pushReplacement(
              MaterialPageRoute(builder: (_) => const CameraView()),
            );
          }
        },
        child: BlocBuilder<LoginCubit, LoginState>(
          builder: (context, state) {
            final cubit = context.read<LoginCubit>();
            return Scaffold(
              body: SafeArea(
                child: Center(
                  child: SingleChildScrollView(
                    padding: const EdgeInsets.all(AppSizes.paddingL),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        const Text(
                          AppStrings.loginTitle,
                          style: TextStyle(
                            fontSize: AppSizes.loginTitleSize,
                            fontWeight: FontWeight.w600,
                            color: AppColors.primary,
                          ),
                        ),

                        const SizedBox(height: AppSizes.paddingS),

                        const Text(
                          AppStrings.loginSubtitle,
                          style: TextStyle(color: AppColors.muted),
                        ),

                        const SizedBox(height: AppSizes.paddingL),

                        LabeledTextField(
                          label: AppStrings.emailLabel,
                          controller: _loginController,
                          textInputAction: TextInputAction.next,
                          onChanged: cubit.loginChanged,
                          hintText: AppStrings.emailHint,
                          errorText: state.loginError,
                        ),

                        const SizedBox(height: AppSizes.paddingL),

                        LabeledTextField(
                          key: const Key('passwordField'),
                          label: AppStrings.passwordLabel,
                          hintText: AppStrings.passwordHint,
                          controller: _passwordController,
                          obscureText: _obscurePassword,
                          textInputAction: TextInputAction.done,
                          onChanged: cubit.passwordChanged,
                          errorText: state.passwordError,
                          onSubmitted: (_) => cubit.login(
                            login: _loginController.text,
                            password: _passwordController.text,
                          ),
                          suffixIcon: IconButton(
                            onPressed: () => setState(
                              () => _obscurePassword = !_obscurePassword,
                            ),
                            icon: Icon(
                              _obscurePassword
                                  ? Icons.visibility_outlined
                                  : Icons.visibility_off_outlined,
                            ),
                          ),
                        ),

                        if (state.errorMessage != null) ...[
                          const SizedBox(height: AppSizes.paddingM),
                          Text(
                            state.errorMessage!,
                            key: const Key('loginError'),
                            style: const TextStyle(
                              color: AppColors.validationError,
                              fontSize: AppSizes.smallTextSize,
                            ),
                          ),
                        ],

                        const SizedBox(height: AppSizes.paddingL),

                        SizedBox(
                          height: AppSizes.loginButtonHeight,
                          child: FilledButton(
                            key: const Key('loginButton'),
                            onPressed: state.status == LoginStatus.loading
                                ? null
                                : () => cubit.login(
                                    login: _loginController.text,
                                    password: _passwordController.text,
                                  ),
                            child: state.status == LoginStatus.loading
                                ? const SizedBox(
                                    width: AppSizes.paddingL,
                                    height: AppSizes.paddingL,
                                    child: CircularProgressIndicator(
                                      strokeWidth: AppSizes.loginLoaderStroke,
                                      color: AppColors.errorBackground,
                                    ),
                                  )
                                : const Text(
                                    AppStrings.loginButton,
                                    style: TextStyle(
                                      fontSize: AppSizes.bodySize,
                                      fontWeight: FontWeight.w600,
                                    ),
                                  ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }
}
