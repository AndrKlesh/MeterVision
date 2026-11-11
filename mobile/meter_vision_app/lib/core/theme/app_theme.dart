import 'package:meter_vision_app/core/theme/app_sizes.dart';
import 'package:flutter/material.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';

ThemeData get appTheme {
  return ThemeData(
    colorScheme: .fromSeed(seedColor: AppColors.seed),
    scaffoldBackgroundColor: AppColors.errorBackground,
    appBarTheme: AppBarThemeData(backgroundColor: AppColors.errorBackground),
    filledButtonTheme: FilledButtonThemeData(
      style: ButtonStyle(
        backgroundColor: WidgetStateProperty.fromMap(
          <WidgetStatesConstraint, Color?>{
            WidgetState.error: AppColors.validationError,
            WidgetState.hovered & WidgetState.focused: AppColors.focusAccent,
            WidgetState.focused: AppColors.primary,
            WidgetState.disabled: AppColors.disabled,
            WidgetState.any: AppColors.primary,
          },
        ),
        foregroundColor: WidgetStateProperty.fromMap(
          <WidgetStatesConstraint, Color?>{
            WidgetState.error: AppColors.validationError,
            WidgetState.hovered & WidgetState.focused: AppColors.focusAccent,
            WidgetState.focused: AppColors.errorBackground,
            WidgetState.disabled: AppColors.muted,
            WidgetState.any: AppColors.errorBackground,
          },
        ),
        shape: WidgetStateProperty.all(
          RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppSizes.radiusS),
          ),
        ),
      ),
    ),
    outlinedButtonTheme: OutlinedButtonThemeData(
      style: ButtonStyle(
        foregroundColor: WidgetStateProperty.fromMap(
          <WidgetStatesConstraint, Color?>{
            WidgetState.error: AppColors.validationError,
            WidgetState.hovered & WidgetState.focused: AppColors.focusAccent,
            WidgetState.focused: AppColors.errorBackground,
            WidgetState.disabled: AppColors.muted,
            WidgetState.any: AppColors.primary,
          },
        ),
        side: WidgetStateBorderSide.fromMap(
          <WidgetStatesConstraint, BorderSide?>{
            WidgetState.error: BorderSide(
              color: AppColors.validationError,
              width: AppSizes.thinStroke,
            ),
            WidgetState.hovered: BorderSide(
              color: AppColors.validationError,
              width: AppSizes.thinStroke,
            ),
            WidgetState.focused: BorderSide(
              color: AppColors.focusBorder,
              width: AppSizes.thinStroke,
            ),
            WidgetState.disabled: BorderSide(
              color: AppColors.muted,
              width: AppSizes.thinStroke,
            ),
            WidgetState.any: BorderSide(
              color: AppColors.primary,
              width: AppSizes.thinStroke,
            ),
          },
        ),
        shape: WidgetStateProperty.all(
          RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppSizes.radiusS),
          ),
        ),
      ),
    ),

    inputDecorationTheme: InputDecorationTheme(
      filled: true,
      fillColor: AppColors.fieldFill,
      contentPadding: const EdgeInsets.symmetric(
        horizontal: AppSizes.paddingM,
        vertical: AppSizes.paddingM,
      ),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppSizes.radiusM),
        borderSide: BorderSide.none,
      ),
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppSizes.radiusM),
        borderSide: BorderSide.none,
      ),

      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppSizes.radiusM),
        borderSide: const BorderSide(
          color: AppColors.primary,
          width: AppSizes.fieldBorderWidth,
        ),
      ),

      errorBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppSizes.radiusM),
        borderSide: const BorderSide(
          color: AppColors.errorBorder,
          width: AppSizes.fieldBorderWidth,
        ),
      ),

      focusedErrorBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppSizes.radiusM),
        borderSide: const BorderSide(
          color: AppColors.errorBorder,
          width: AppSizes.fieldBorderWidth,
        ),
      ),

      errorStyle: const TextStyle(fontSize: AppSizes.captionSize),

      hintStyle: const TextStyle(color: AppColors.hint),
    ),
  );
}
