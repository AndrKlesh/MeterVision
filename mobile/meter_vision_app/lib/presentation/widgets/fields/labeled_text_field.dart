import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class LabeledTextField extends StatelessWidget {
  const LabeledTextField({
    super.key,
    required this.label,
    required this.controller,
    this.hintText,
    this.keyboardType,
    this.inputFormatters,
    this.errorText,
    this.onChanged,
    this.textInputAction,
    this.focusNode,
    this.obscureText = false,
    this.suffixIcon,
    this.onSubmitted,
  });

  /// Заголовок над полем (например, AppStrings.readingLabel).
  final String label;

  /// Контроллер для управления текстом.
  final TextEditingController controller;

  /// Подсказка внутри поля (появляется, когда текст пуст).
  final String? hintText;

  /// Тип клавиатуры (например, цифровая для показаний).
  final TextInputType? keyboardType;

  /// Форматтеры (например, запрет ввода букв для цифр).
  final List<TextInputFormatter>? inputFormatters;

  /// Текст ошибки (например, при валидации). Если null — ошибки нет.
  final String? errorText;

  /// Колбэк при изменении текста.
  final ValueChanged<String>? onChanged;

  final TextInputAction? textInputAction;
  final FocusNode? focusNode;
  final bool obscureText;
  final Widget? suffixIcon;
  final ValueChanged<String>? onSubmitted;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      mainAxisSize: MainAxisSize.min,
      children: [
        Text(
          label,
          style: const TextStyle(
            fontSize: AppSizes.smallTextSize,
            color: AppColors.label,
            fontWeight: FontWeight.w400,
          ),
        ),
        const SizedBox(height: AppSizes.paddingS),

        TextField(
          controller: controller,
          focusNode: focusNode,
          keyboardType: keyboardType,
          inputFormatters: inputFormatters,
          onChanged: onChanged,
          obscureText: obscureText,
          onSubmitted: onSubmitted,
          textInputAction: textInputAction,
          style: const TextStyle(
            fontSize: AppSizes.bodySize,
            fontWeight: FontWeight.w500,
            color: AppColors.textPrimary,
            letterSpacing: AppSizes.fieldLetterSpacing,
          ),
          decoration: InputDecoration(
            hintText: hintText,
            errorText: errorText,
            suffixIcon: suffixIcon,
          ),
        ),
      ],
    );
  }
}
