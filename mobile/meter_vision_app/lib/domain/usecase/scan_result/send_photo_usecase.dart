import 'package:meter_vision_app/domain/datasources/meter_service/i_meter_service.dart';
import 'package:meter_vision_app/domain/entity/recognition_entity.dart';

class SendPhotoUseCase {
  const SendPhotoUseCase(this.service);
  final IMeterService service;
  Future<Recognition> call(String path) async {
    var result = await _PhotoRecognitionGateway(service).recognize(path);
    return result;
  }
}

class _PhotoRecognitionGateway {
  const _PhotoRecognitionGateway(this.service);
  final IMeterService service;

  Future<Recognition> recognize(String path) => service.recognize(path);
}
