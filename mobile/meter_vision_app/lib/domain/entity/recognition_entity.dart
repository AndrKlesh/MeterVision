import 'package:equatable/equatable.dart';

/// Результат распознавания показаний счётчика
class Recognition extends Equatable {
  /// id результата распознавания
  final String id;

  /// распознанные показания
  final String value;

  /// серийный номер
  final String serial;

  /// уверенность распознавания
  final double confidence;

  const Recognition({
    required this.id,
    required this.value,
    required this.serial,
    required this.confidence,
  });

  factory Recognition.fromJson(Map<String, dynamic> json) {
    return Recognition(
      id: (json['id'] ?? '')?.toString() ?? '',
      value: (json['reading'] ?? json['value'] ?? '').toString(),
      serial: (json['serial'] ?? json['serialNumber'] ?? '').toString(),
      confidence: double.tryParse('${json['confidence'] ?? 0}') ?? 0.0,
    );
  }

  @override
  String toString() =>
      'Recognition(id: $id, reading: $value, serial: $serial, '
      'confidence: $confidence)';

  @override
  List<Object?> get props => [id, value, serial, confidence];
}
