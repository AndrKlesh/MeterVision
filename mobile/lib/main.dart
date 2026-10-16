import 'package:flutter/material.dart';
import 'features/auth/presentation/login_screen.dart';

void main() {
  runApp(const MeterVisionApp());
}

class MeterVisionApp extends StatelessWidget {
  const MeterVisionApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'MeterVision',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFF2563EB),
        ),
      ),
      home: const LoginScreen(),
    );
  }
}
