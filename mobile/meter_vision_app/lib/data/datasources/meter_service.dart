import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:meter_vision_app/core/strings/app_strings.dart';

import 'dart:io';

import 'package:meter_vision_app/domain/datasources/meter_service/i_meter_service.dart';
import 'package:meter_vision_app/domain/entity/recognition_entity.dart';

class MeterService implements IMeterService {
  static const String _baseUrl = 'http://localhost:5100';
  final http.Client _client = http.Client();

  @override
  Future<Recognition> recognize(String imagePath) async {
    final file = File(imagePath);

    if (!await file.exists()) {
      throw const FormatException(AppStrings.emptyPhoto);
    }

    final bytes = await file.readAsBytes();

    if (bytes.isEmpty) {
      throw const FormatException(AppStrings.emptyPhoto);
    }

    final request = http.MultipartRequest(
      'POST',
      Uri.parse('$_baseUrl/api/v1/recognition/upload'),
    );

    request.files.add(
      http.MultipartFile.fromBytes(
        'photo',
        bytes,
        filename: file.uri.pathSegments.last,
      ),
    );

    try {
      final streamedResponse = await _client.send(request);
      final response = await http.Response.fromStream(streamedResponse);

      if (response.statusCode != 200) {
        throw Exception(
          'Ошибка распознавания: ${response.statusCode}\n${response.body}',
        );
      }

      final json = jsonDecode(response.body) as Map<String, dynamic>;

      return Recognition(
        id: json['id'] as String,
        value: json['value'] as String,
        serial: json['serialNumber'] as String,
        confidence: (json['confidence'] as num).toDouble(),
      );
    } catch (e) {
      rethrow;
    }
  }

  @override
  Future<void> confirm(String id, String reading, String serial) async {
    final response = await _client.post(
      Uri.parse('$_baseUrl/api/v1/readings/confirm'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({
        'id': id,
        'serialNumber': serial,
        'value': double.parse(reading),
      }),
    );

    if (response.statusCode != 200) {
      throw Exception(
        'Ошибка подтверждения показаний: '
        '${response.statusCode}\n${response.body}',
      );
    }
  }
}
