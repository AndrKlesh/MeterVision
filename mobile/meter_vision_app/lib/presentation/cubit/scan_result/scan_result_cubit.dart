import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:meter_vision_app/domain/entity/recognition_entity.dart';
import 'package:meter_vision_app/presentation/cubit/scan_result/scan_status.dart';

import 'package:meter_vision_app/domain/usecase/scan_result/confirm_reading_usecase.dart';

part 'scan_result_state.dart';

class ScanResultCubit extends Cubit<ScanResultState> {
  ScanResultCubit(Recognition result, this._confirm)
    : _id = result.id,
      super(
        ScanResultState(
          measuresValue: result.value,
          serialNumber: result.serial,
        ),
      );

  /// Обновление поля AppStrings.readingLabel
  void measuresChanged(String value) {
    emit(
      state.copyWith(
        measuresValue: value,
        clearMeasuresError: true,
        clearError: true,
      ),
    );
  }

  /// Обновление поля AppStrings.serialLabel
  void serialChanged(String value) {
    _serialDraft = value;
    emit(
      state.copyWith(
        serialNumber: value,
        clearSerialError: true,
        clearError: true,
      ),
    );
  }

  /// Отправка данных (заглушка)
  Future<void> submit() async {
    if (state.isLoading) return;

    final measuresError = _validateMeasures(state.measuresValue);
    final serialError = _validateSerial(state.serialNumber);

    if (measuresError != null || serialError != null) {
      emit(
        state.copyWith(
          measuresError: measuresError,
          serialError: serialError,
          status: ScanStatus.initial,
        ),
      );
      return;
    }

    emit(state.copyWith(status: ScanStatus.loading, clearError: true));

    try {
      await _confirm(_id, state.measuresValue.trim(), _serialDraft.trim());

      if (isClosed) return;
      emit(state.copyWith(status: ScanStatus.success));
    } catch (_) {
      if (isClosed) return;
      emit(
        state.copyWith(
          status: ScanStatus.error,
          errorMessage: AppStrings.submitFailed,
        ),
      );
    }
  }

  String? _validateMeasures(String value) {
    if (value.trim().isEmpty) return AppStrings.emptyReading;
    return null;
  }

  String? _validateSerial(String value) {
    if (value.trim().isEmpty) return AppStrings.emptySerial;
    if (value.trim().length < 10) return AppStrings.shortSerial;
    return null;
  }

  final ConfirmReadingUseCase _confirm;
  final String _id;
  late String _serialDraft = state.serialNumber;
}
