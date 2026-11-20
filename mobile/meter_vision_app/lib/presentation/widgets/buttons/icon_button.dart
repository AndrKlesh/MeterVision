import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';

/// Кнопка-картинка из макета Figma: SVG уже содержит и круг, и иконку.
class SvgIconButton extends StatelessWidget {
  const SvgIconButton({
    super.key,
    required this.asset,
    required this.onPressed,
    this.size = AppSizes.circleButtonSize,
    this.backgroundColor = AppColors.errorBackground,
    this.iconColor = AppColors.black,
  });

  final String asset;
  final VoidCallback? onPressed;
  final double size;
  final Color backgroundColor;
  final Color iconColor;

  @override
  Widget build(BuildContext context) {
    return Opacity(
      opacity: onPressed == null ? 0.5 : 1,
      child: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onTap: onPressed,
        child: Container(
          width: size,
          height: size,
          decoration: BoxDecoration(
            color: backgroundColor,
            shape: BoxShape.circle,
          ),
          alignment: Alignment.center,
          child: Padding(
            padding: const EdgeInsets.all(AppSizes.paddingS),
            child: SvgPicture.asset(
              asset,
              colorFilter: ColorFilter.mode(iconColor, BlendMode.srcIn),
              width: size,
              height: size,
            ),
          ),
        ),
      ),
    );
  }
}
