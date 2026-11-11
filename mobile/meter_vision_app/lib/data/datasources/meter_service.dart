import 'package:meter_vision_app/core/strings/app_strings.dart';

import 'dart:io';

import 'package:meter_vision_app/domain/datasources/meter_service/i_meter_service.dart';
import 'package:meter_vision_app/domain/entity/recognition_entity.dart';

class MeterService implements IMeterService {
  int _photoId = 1;
  final _confirmed = <int, (String, String)>{};

  @override
  Future<Recognition> recognize(String imagePath) async {
    final bytes = await File(imagePath).readAsBytes();
    if (bytes.isEmpty) throw const FormatException(AppStrings.emptyPhoto);
    await Future<void>.delayed(const Duration(milliseconds: 500));
    return (
      id: _photoId++,
      reading: '00124.68',
      serial: '5083917624',
      confidence: 87.0,
    );
  }

  @override
  Future<void> confirm(int id, String reading, String serial) async {
    if (id < 1 || id >= _photoId) throw StateError(AppStrings.unknownPhoto);
    await Future<void>.delayed(const Duration(milliseconds: 400));
    _confirmed[id] = (reading, serial);
  }
}
