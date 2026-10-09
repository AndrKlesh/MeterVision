import 'package:flutter/material.dart';
import 'package:meter_vision_app/core/theme/app_theme.dart';
import 'package:meter_vision_app/presentation/views/example_view.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: appTheme,
      home: const ExampleView(title: 'Flutter Demo Home Page'),
    );
  }
}
