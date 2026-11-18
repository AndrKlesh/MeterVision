import 'package:meter_vision_app/domain/entity/recognition_entity.dart';

abstract interface class IMeterService {
  Future<Recognition> recognize(String imagePath);
  Future<void> confirm(String id, String reading, String serial);
}
