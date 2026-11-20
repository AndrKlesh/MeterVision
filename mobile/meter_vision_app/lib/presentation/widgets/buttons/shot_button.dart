import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:flutter/material.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';

class ShotButton extends StatelessWidget {
  const ShotButton({
    super.key,
    required this.onPressed,
    this.size = 72,
    this.isLoading = false,
    this.isEnabled = true,
  });

  /// Колбэк съёмки
  final VoidCallback? onPressed;

  /// Диаметр кнопки
  final double size;

  /// Индикатор загрузки
  final bool isLoading;

  /// Индикатор доступности кнопки
  final bool isEnabled;

  bool get _canTap => isEnabled && !isLoading && onPressed != null;

  @override
  Widget build(BuildContext context) {
    return Opacity(
      opacity: _canTap ? 1 : 0.5,
      child: Semantics(
        button: true,
        label: AppStrings.takePhoto,
        child: Material(
          color: AppColors.transparent,
          shape: const CircleBorder(),
          clipBehavior: Clip.antiAlias,
          child: InkWell(
            onTap: _canTap ? onPressed : null,
            customBorder: const CircleBorder(),
            child: Container(
              width: size,
              height: size,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: AppColors.errorBackground,
                boxShadow: [
                  BoxShadow(
                    color: AppColors.errorBackground.withValues(alpha: 0.35),
                  ),
                ],
              ),
              alignment: Alignment.center,
              child: isLoading
                  ? SizedBox(
                      width: AppSizes.shutterLoaderSize,
                      height: AppSizes.shutterLoaderSize,
                      child: const CircularProgressIndicator(
                        color: AppColors.primary,
                        strokeWidth: AppSizes.shutterStroke,
                      ),
                    )
                  : Icon(
                      Icons.photo_camera_outlined,
                      color: AppColors.errorBackground,
                      size: size * 0.45,
                    ),
            ),
          ),
        ),
      ),
    );
  }
}
