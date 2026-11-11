import 'package:meter_vision_app/domain/entity/recognition_entity.dart';
import 'package:meter_vision_app/domain/usecase/scan_result/confirm_reading_usecase.dart';

import 'dart:io';

import 'package:meter_vision_app/core/theme/app_colors.dart';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';
import 'package:meter_vision_app/presentation/cubit/scan_result/scan_result_cubit.dart';
import 'package:meter_vision_app/presentation/widgets/cards/recognition_card.dart';
import 'package:meter_vision_app/presentation/widgets/fields/labeled_text_field.dart';

class ScanResultView extends StatefulWidget {
  const ScanResultView({
    super.key,
    required this.imagePath,
    required this.result,
  });

  final String imagePath;
  final Recognition result;

  @override
  State<ScanResultView> createState() => _ScanResultViewState();
}

class _ScanResultViewState extends State<ScanResultView> {
  late final measures = TextEditingController(text: widget.result.reading);
  late final serial = TextEditingController(text: widget.result.serial);

  @override
  void dispose() {
    measures.dispose();
    serial.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) =>
          ScanResultCubit(widget.result, context.read<ConfirmReadingUseCase>()),
      child: BlocListener<ScanResultCubit, ScanResultState>(
        listener: (context, state) {
          if (state.errorMessage != null) {
            ScaffoldMessenger.of(context)
                .showSnackBar(SnackBar(content: Text(state.errorMessage!)));
          }
          if (state.isSuccess) {
            ScaffoldMessenger.of(context).showSnackBar(
              const SnackBar(content: Text(AppStrings.readingsSent)),
            );
            Navigator.of(context).pop(true);
          }
        },
        child: BlocBuilder<ScanResultCubit, ScanResultState>(
          builder: (context, state) {
            final cubit = context.read<ScanResultCubit>();
            return PopScope(
              canPop: !state.isLoading,
              child: Scaffold(
                appBar: AppBar(title: Text(AppStrings.scanResultTitle)),
                resizeToAvoidBottomInset: true,
                body: SafeArea(
                  child: Column(
                    children: [
                      Expanded(
                        child: AbsorbPointer(
                          absorbing: state.isLoading,
                          child: SingleChildScrollView(
                            padding: EdgeInsets.all(AppSizes.paddingM),
                            child: Column(
                              children: [
                                GestureDetector(
                                  onTap: () {
                                    showDialog(
                                      context: context,
                                      builder: (_) => Dialog(
                                        backgroundColor: AppColors.transparent,
                                        insetPadding: EdgeInsets.all(
                                          AppSizes.paddingS,
                                        ),
                                        elevation: 0,
                                        child: Stack(
                                          alignment: Alignment.center,
                                          children: [
                                            InteractiveViewer(
                                              minScale: 1.0,
                                              maxScale: 4.0,
                                              child: Image.file(
                                                File(widget.imagePath),
                                                fit: BoxFit.contain,
                                              ),
                                            ),

                                            Positioned(
                                              top: AppSizes.paddingM,
                                              right: AppSizes.paddingM,
                                              child: IconButton(
                                                icon: const Icon(
                                                  Icons.close,
                                                  color:
                                                      AppColors.errorBackground,
                                                  size: AppSizes.sheetIconSize,
                                                ),
                                                onPressed: () =>
                                                    Navigator.of(context).pop(),
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),
                                    );
                                  },
                                  child: ClipRRect(
                                    borderRadius: BorderRadius.circular(
                                      AppSizes.radiusM,
                                    ),
                                    child: Image.file(
                                      File(widget.imagePath),
                                      width: double.infinity,
                                      height: AppSizes.resultImageHeight,
                                      fit: BoxFit.cover,
                                    ),
                                  ),
                                ),

                                SizedBox(height: AppSizes.paddingS),

                                ConfidenceIndicator(
                                  confidence: widget.result.confidence,
                                ),

                                SizedBox(height: AppSizes.paddingS),

                                LabeledTextField(
                                  label: AppStrings.readingLabel,
                                  controller: measures,
                                  onChanged: cubit.measuresChanged,
                                  errorText: state.measuresError,
                                ),

                                SizedBox(height: AppSizes.paddingS),

                                LabeledTextField(
                                  label: AppStrings.serialLabel,
                                  controller: serial,
                                  onChanged: cubit.serialChanged,
                                  errorText: state.serialError,
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),

                      Padding(
                        padding: EdgeInsetsGeometry.symmetric(
                          horizontal: AppSizes.paddingM,
                          vertical: AppSizes.paddingM,
                        ),
                        child: Column(
                          crossAxisAlignment: .stretch,
                          children: [
                            SizedBox(
                              height: AppSizes.smallButtonSize,
                              child: FilledButton(
                                onPressed: state.isLoading
                                    ? null
                                    : () {
                                        cubit.submit();
                                      },
                                child: Text(
                                  state.isLoading
                                      ? AppStrings.sending
                                      : AppStrings.submitReadings,
                                ),
                              ),
                            ),

                            SizedBox(height: AppSizes.paddingS),

                            SizedBox(
                              height: AppSizes.smallButtonSize,
                              child: OutlinedButton(
                                onPressed: state.isLoading
                                    ? null
                                    : () => Navigator.of(context).pop(),
                                child: Text(AppStrings.cancel),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
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
