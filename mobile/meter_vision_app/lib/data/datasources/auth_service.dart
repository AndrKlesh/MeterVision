import 'dart:convert';

import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/domain/datasources/auth/i_auth_api_service.dart';
import 'package:http/http.dart' as http;

class AuthService extends IAuthService {
  static const String _baseUrl = 'http://localhost:5100';

  @override
  Future<void> login(String login, String password) async {
    final response = await http.get(
      Uri.parse('$_baseUrl/api/v1/login'),
      headers: {
        'Content-Type': 'application/json',
        'Authorization':
            'Basic ${base64Encode(utf8.encode('$login:$password'))}',
      },
    );

    if (response.statusCode == 401) {
      throw Exception(AppStrings.invalidCredentials);
    }

    if (response.statusCode != 200) {
      throw Exception('Ошибка авторизации: ${response.request}');
    }
  }

  @override
  Future<void> logout() async {}
}
