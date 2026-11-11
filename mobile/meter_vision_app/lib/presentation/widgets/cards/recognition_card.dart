import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';
import 'package:flutter/material.dart';

class ConfidenceIndicator extends StatelessWidget {
  const ConfidenceIndicator({super.key, required this.confidence})
    : assert(confidence >= 0 && confidence <= 100);

  final double confidence;

  @override
  Widget build(BuildContext context) {
    final status = _getStatus(confidence);

    return Container(
      padding: const EdgeInsets.all(AppSizes.paddingM),
      decoration: BoxDecoration(
        color: status.backgroundColor,
        borderRadius: BorderRadius.circular(AppSizes.radiusM),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.baseline,
            textBaseline: TextBaseline.alphabetic,
            children: [
              const Expanded(
                child: Text(
                  AppStrings.confidenceTitle,
                  style: TextStyle(
                    fontSize: AppSizes.bodySize,
                    fontWeight: FontWeight.w600,
                    color: AppColors.textPrimary,
                  ),
                ),
              ),
              Text(
                AppStrings.percentage(confidence.toInt()),
                style: const TextStyle(
                  fontSize: AppSizes.confidenceSize,
                  fontWeight: FontWeight.w700,
                  color: AppColors.textPrimary,
                ),
              ),
            ],
          ),
          const SizedBox(height: AppSizes.paddingCompact),

          ClipRRect(
            borderRadius: BorderRadius.circular(AppSizes.progressRadius),
            child: LinearProgressIndicator(
              value: confidence / 100,
              minHeight: AppSizes.progressHeight,
              backgroundColor: AppColors.progressTrack,
              valueColor: AlwaysStoppedAnimation<Color>(status.progressColor),
            ),
          ),
          const SizedBox(height: AppSizes.paddingCompact),

          Text(
            status.description,
            style: TextStyle(
              fontSize: AppSizes.smallTextSize,
              color: status.textColor,
              height: AppSizes.bodyLineHeight,
            ),
          ),
        ],
      ),
    );
  }

  _ConfidenceStatus _getStatus(double value) {
    if (value >= 80) {
      return const _ConfidenceStatus(
        backgroundColor: AppColors.highBackground,
        progressColor: AppColors.highProgress,
        textColor: AppColors.textSecondary,
        description: AppStrings.confidenceHigh,
      );
    } else if (value >= 50) {
      return const _ConfidenceStatus(
        backgroundColor: AppColors.mediumBackground,
        progressColor: AppColors.mediumProgress,
        textColor: AppColors.textSecondary,
        description: AppStrings.confidenceMedium,
      );
    } else if (value >= 30) {
      return const _ConfidenceStatus(
        backgroundColor: AppColors.lowBackground,
        progressColor: AppColors.lowProgress,
        textColor: AppColors.textPrimary,
        description: AppStrings.confidenceLow,
      );
    } else {
      return const _ConfidenceStatus(
        backgroundColor: AppColors.criticalBackground,
        progressColor: AppColors.errorBorder,
        textColor: AppColors.criticalText,
        description: AppStrings.confidenceCritical,
      );
    }
  }
}

class _ConfidenceStatus {
  const _ConfidenceStatus({
    required this.backgroundColor,
    required this.progressColor,
    required this.textColor,
    required this.description,
  });

  final Color backgroundColor;
  final Color progressColor;
  final Color textColor;
  final String description;
}
