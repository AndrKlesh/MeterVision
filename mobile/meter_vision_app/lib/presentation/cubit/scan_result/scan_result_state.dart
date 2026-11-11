part of 'scan_result_cubit.dart';

class ScanResultState extends Equatable {
  final ScanStatus status;
  final String measuresValue;
  final String serialNumber;
  final String? measuresError;
  final String? serialError;
  final String? errorMessage;

  const ScanResultState({
    this.status = ScanStatus.initial,
    this.measuresValue = '',
    this.serialNumber = '',
    this.measuresError,
    this.serialError,
    this.errorMessage,
  });

  bool get isLoading => status == ScanStatus.loading;
  bool get isSuccess => status == ScanStatus.success;

  ScanResultState copyWith({
    ScanStatus? status,
    String? measuresValue,
    String? serialNumber,
    String? errorMessage,

    String? measuresError,
    String? serialError,

    bool clearMeasuresError = false,
    bool clearSerialError = false,

    bool clearError = false,
  }) {
    return ScanResultState(
      status: status ?? this.status,
      measuresValue: measuresValue ?? this.measuresValue,
      serialNumber: serialNumber ?? this.serialNumber,

      measuresError: clearMeasuresError
          ? null
          : (measuresError ?? this.measuresError),
      serialError: clearSerialError ? null : (serialError ?? this.serialError),

      errorMessage: clearError ? null : (errorMessage ?? this.errorMessage),
    );
  }

  @override
  List<Object?> get props => [
    status,
    measuresValue,
    serialNumber,
    measuresError,
    serialError,
    errorMessage,
  ];
}
