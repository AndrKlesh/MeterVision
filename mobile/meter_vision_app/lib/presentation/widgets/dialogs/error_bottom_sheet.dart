import 'package:flutter/material.dart';
import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';


Future<void> showErrorBottomSheet(BuildContext context, String message) {
  return showModalBottomSheet<void>(
    context: context,
    backgroundColor: AppColors.errorBackground,
    shape: const RoundedRectangleBorder(
      borderRadius: BorderRadius.vertical(
        top: Radius.circular(AppSizes.radiusL),
      ),
    ),
    builder: (sheetContext) {
      return Padding(
        padding: const EdgeInsets.all(AppSizes.paddingL),
        child: SafeArea(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const Icon(
                Icons.error_outline,
                color: AppColors.error,
                size: AppSizes.sheetIconSize,
              ),
              const SizedBox(height: AppSizes.radiusM),
              Text(
                message,
                textAlign: TextAlign.center,
                style: const TextStyle(fontSize: 16, color: AppColors.error),
              ),
              const SizedBox(height: AppSizes.radiusL),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: AppColors.primary,
                    foregroundColor: Colors.white,
                  ),
                  onPressed: () => Navigator.pop(sheetContext),
                  child: const Text(AppStrings.cameraGotIt),
                ),
              ),
            ],
          ),
        ),
      );
    },
  );
}
