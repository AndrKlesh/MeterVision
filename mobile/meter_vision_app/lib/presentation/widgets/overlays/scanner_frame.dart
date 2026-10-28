import 'dart:math' as math;

import 'package:flutter/material.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';


class ScannerFrame extends StatelessWidget {
  const ScannerFrame({super.key});

  @override
  Widget build(BuildContext context) {
    return IgnorePointer(
      child: LayoutBuilder(
        builder: (context, constraints) {
          final width = constraints.maxWidth - 2 * AppSizes.paddingM;
          final height = width / AppSizes.frameAspectRatio;
          final rect = Rect.fromLTWH(
            AppSizes.paddingM,
            AppSizes.frameTop,
            width,
            height,
          );
          return CustomPaint(
            size: Size.infinite,
            painter: _ScannerFramePainter(rect),
          );
        },
      ),
    );
  }
}

class _ScannerFramePainter extends CustomPainter {
  _ScannerFramePainter(this.rect);

  final Rect rect;

  @override
  void paint(Canvas canvas, Size size) {
    final hole = RRect.fromRectAndRadius(
      rect,
      const Radius.circular(AppSizes.frameRadius),
    );

    
    final overlay = Path()
      ..fillType = PathFillType.evenOdd
      ..addRect(Offset.zero & size)
      ..addRRect(hole);
    canvas.drawPath(overlay, Paint()..color = AppColors.overlay);

   
    final borderPaint = Paint()
      ..style = PaintingStyle.stroke
      ..strokeWidth = AppSizes.frameBorderWidth
      ..color = AppColors.accent;

    final borderPath = Path()
      ..addRRect(hole.deflate(AppSizes.frameBorderWidth / 2));

    for (final metric in borderPath.computeMetrics()) {
      var distance = 0.0;
      while (distance < metric.length) {
        final end = math.min(distance + AppSizes.frameDash, metric.length);
        canvas.drawPath(metric.extractPath(distance, end), borderPaint);
        distance += AppSizes.frameDash + AppSizes.frameGap;
      }
    }
  }

  @override
  bool shouldRepaint(covariant _ScannerFramePainter oldDelegate) =>
      oldDelegate.rect != rect;
}
