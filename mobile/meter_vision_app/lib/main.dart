import 'package:flutter/material.dart';
import 'package:meter_vision_app/core/theme/app_theme.dart';
import 'package:meter_vision_app/presentation/views/login/login_view.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'MeterVision',
      debugShowCheckedModeBanner: false,
      theme: appTheme,
      home: const LoginView(),
    );
  }
}
