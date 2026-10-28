import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';

/// Кнопка-картинка из макета Figma: SVG уже содержит и круг, и иконку.
class SvgButton extends StatelessWidget {
  const SvgButton({
    super.key,
    required this.asset,
    required this.onPressed,
    this.size = AppSizes.circleButtonSize,
  });

  final String asset;
  final VoidCallback? onPressed;
  final double size;

  @override
  Widget build(BuildContext context) {
    return Opacity(
      opacity: onPressed == null ? 0.5 : 1,
      child: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onTap: onPressed,
        child: SizedBox(
          width: size,
          height: size,
          child: SvgPicture.asset(asset, width: size, height: size),
        ),
      ),
    );
  }
}