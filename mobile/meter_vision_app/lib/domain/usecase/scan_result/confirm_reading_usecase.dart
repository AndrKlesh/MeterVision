import 'package:meter_vision_app/domain/datasources/meter_service/i_meter_service.dart';

class ConfirmReadingUseCase {
  const ConfirmReadingUseCase(this.service);

  final IMeterService service;

  Future<void> call(int id, String reading, String serial) async {
    await service.confirm(
      id,
      reading,
      int.tryParse(serial)?.toString() ?? serial,
    );
  }
}
