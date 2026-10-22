import 'dart:io';

import 'package:camera/camera.dart';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:permission_handler/permission_handler.dart';

export 'package:image_picker/image_picker.dart' show XFile;

const Color _primaryColor = Color(0xFF0059A9);
const Color _accentColor = Color(0xFF1793FF);
const Color _errorColor = Color(0xFFB3261E);
const Color _errorBackgroundColor = Color(0xFFFDF1F0);

const int _maxPhotoBytes = 10 * 1024 * 1024;
const List<String> _allowedExtensions = ['jpg', 'jpeg', 'png', 'heic', 'heif'];

/// Состояние камеры, которое видит экран.
enum CameraStatus {
  loading,
  ready,
  permissionDenied,
  permissionPermanentlyDenied,
  unavailable,
}


String? validatePhoto(String path, int size) {
  final dot = path.lastIndexOf('.');
  final ext = dot == -1 ? '' : path.substring(dot + 1).toLowerCase();
  if (!_allowedExtensions.contains(ext)) {
    return 'Неподдерживаемый формат. Допустимы JPG, PNG, HEIC.';
  }
  if (size == 0) return 'Снимок пустой. Сфотографируйте ещё раз.';
  if (size > _maxPhotoBytes) return 'Снимок слишком большой. Максимум 10 МБ.';
  return null;
}

/// Всё, что экран умеет делать с камерой. 
abstract class CameraService {
  Future<CameraStatus> initialize();
  Future<XFile> takePicture();
  Future<XFile?> pickFromGallery();
  Future<int> sizeOf(XFile file);
  Future<void> setFlash(bool on);
  Future<void> switchCamera();
  Future<void> openSettings();
  Widget buildPreview();
  Widget buildPhoto(XFile file);
  Future<void> dispose();
}


class RealCameraService implements CameraService {
  CameraController? _controller;
  List<CameraDescription> _cameras = [];

  @override
  Future<CameraStatus> initialize() async {
    final permission = await Permission.camera.request();
    if (permission.isPermanentlyDenied) {
      return CameraStatus.permissionPermanentlyDenied;
    }
    if (!permission.isGranted) return CameraStatus.permissionDenied;

    try {
      _cameras = await availableCameras();
      if (_cameras.isEmpty) return CameraStatus.unavailable;

      final back = _cameras.firstWhere(
        (c) => c.lensDirection == CameraLensDirection.back,
        orElse: () => _cameras.first,
      );
      await _start(back);
      return CameraStatus.ready;
    } catch (_) {
      return CameraStatus.unavailable;
    }
  }

  Future<void> _start(CameraDescription description) async {
    await _controller?.dispose();
    final controller = CameraController(
      description,
      ResolutionPreset.high,
      enableAudio: false,
    );
    await controller.initialize();
    _controller = controller;
  }

  @override
  Future<XFile> takePicture() async {
    final controller = _controller;
    if (controller == null) throw StateError('Камера не запущена');
    return controller.takePicture();
  }

  @override
  Future<XFile?> pickFromGallery() {
    return ImagePicker().pickImage(source: ImageSource.gallery);
  }

  @override
  Future<int> sizeOf(XFile file) => file.length();

  @override
  Future<void> setFlash(bool on) async {
    await _controller?.setFlashMode(on ? FlashMode.torch : FlashMode.off);
  }

  @override
  Future<void> switchCamera() async {
    final current = _controller?.description;
    if (current == null || _cameras.length < 2) return;
    final next = _cameras.firstWhere(
      (c) => c.lensDirection != current.lensDirection,
      orElse: () => current,
    );
    await _start(next);
  }

  @override
  Future<void> openSettings() async {
    await openAppSettings();
  }

  @override
  Widget buildPreview() {
    final controller = _controller;
    if (controller == null || !controller.value.isInitialized) {
      return const ColoredBox(color: Color(0xFF424B50));
    }
    final size = controller.value.previewSize!;
    return ClipRect(
      child: FittedBox(
        fit: BoxFit.cover,
        child: SizedBox(
          width: size.height,
          height: size.width,
          child: CameraPreview(controller),
        ),
      ),
    );
  }

  @override
  Widget buildPhoto(XFile file) {
    return Image.file(File(file.path), fit: BoxFit.cover);
  }

  @override
  Future<void> dispose() async {
    await _controller?.dispose();
    _controller = null;
  }
}


class CameraScreen extends StatefulWidget {
  const CameraScreen({super.key, this.service, this.onPhotoReady});

 
  final CameraService? service;

  
  final void Function(XFile photo)? onPhotoReady;

  @override
  State<CameraScreen> createState() => _CameraScreenState();
}

class _CameraScreenState extends State<CameraScreen> {
  late final CameraService _service;

  CameraStatus _status = CameraStatus.loading;
  bool _torchOn = false;
  bool _busy = false;
  bool _uploading = false;
  XFile? _captured;

  @override
  void initState() {
    super.initState();
    _service = widget.service ?? RealCameraService();
    _init();
  }

  @override
  void dispose() {
    _service.dispose();
    super.dispose();
  }

  Future<void> _init() async {
    setState(() => _status = CameraStatus.loading);
    CameraStatus status;
    try {
      status = await _service.initialize();
    } catch (_) {
      status = CameraStatus.unavailable;
    }
    if (!mounted) return;
    setState(() => _status = status);
  }

  Future<void> _showMessage(String message) {
    return showModalBottomSheet<void>(
      context: context,
      backgroundColor: _errorBackgroundColor,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (sheetContext) {
        return Padding(
          padding: const EdgeInsets.all(24),
          child: SafeArea(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                const Icon(Icons.error_outline, color: _errorColor, size: 32),
                const SizedBox(height: 12),
                Text(
                  message,
                  key: const Key('sheet_message'),
                  textAlign: TextAlign.center,
                  style: const TextStyle(fontSize: 16, color: _errorColor),
                ),
                const SizedBox(height: 20),
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      backgroundColor: _primaryColor,
                      foregroundColor: Colors.white,
                    ),
                    onPressed: () => Navigator.pop(sheetContext),
                    child: const Text('Понятно'),
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }

  Future<void> _toggleFlash() async {
    final next = !_torchOn;
    try {
      await _service.setFlash(next);
      if (mounted) setState(() => _torchOn = next);
    } catch (_) {
      await _showMessage('Фонарик недоступен на этом устройстве.');
    }
  }

  Future<void> _switchCamera() async {
    if (_busy) return;
    try {
      await _service.switchCamera();
    } catch (_) {
      await _showMessage('Не удалось переключить камеру.');
    }
  }

  Future<void> _takePhoto() async {
    if (_busy || _status != CameraStatus.ready) return;
    setState(() => _busy = true);
    try {
      final photo = await _service.takePicture();
      await _checkAndKeep(photo);
    } catch (_) {
      await _showMessage('Не удалось сделать снимок. Попробуйте ещё раз.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _openGallery() async {
    if (_busy) return;
    setState(() => _busy = true);
    try {
      final photo = await _service.pickFromGallery();
      if (photo != null) await _checkAndKeep(photo);
    } catch (_) {
      await _showMessage('Не удалось открыть галерею.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _checkAndKeep(XFile photo) async {
    final size = await _service.sizeOf(photo);
    final problem = validatePhoto(photo.path, size);
    if (!mounted) return;
    if (problem != null) {
      await _showMessage(problem);
      return;
    }
    setState(() => _captured = photo);
  }

  void _retake() => setState(() => _captured = null);

  Future<void> _usePhoto() async {
    if (_uploading) return;

    final photo = _captured;
    if (photo == null) return;

    setState(() => _uploading = true);

    try {
      // Временная имитация отправки: сервер пока не подключён.
      await Future<void>.delayed(const Duration(milliseconds: 700));
    } finally {
      if (mounted) setState(() => _uploading = false);
    }

    if (!mounted) return;

    // Сохраняем существующий способ передать фотографию наружу.
    widget.onPhotoReady?.call(photo);

    if (!mounted) return;
    await _showMessage('Отправка фото пока не реализована.');
  }

  @override
  Widget build(BuildContext context) {
    final captured = _captured;
    return Scaffold(
      backgroundColor: Colors.black,
      body: SafeArea(
        child: captured != null
            ? _buildPreview(captured)
            : _buildCameraBody(),
      ),
    );
  }

  Widget _buildCameraBody() {
    switch (_status) {
      case CameraStatus.loading:
        return const Center(
          child: CircularProgressIndicator(color: Colors.white),
        );
      case CameraStatus.ready:
        return _buildViewfinder();
      case CameraStatus.permissionDenied:
        return _buildNoAccess(
          text: 'Разрешите доступ к камере, чтобы сфотографировать счётчик',
          buttonText: 'Разрешить',
          onPressed: _init,
        );
      case CameraStatus.permissionPermanentlyDenied:
        return _buildNoAccess(
          text: 'Доступ к камере запрещён. Разрешите его в настройках',
          buttonText: 'Открыть настройки',
          onPressed: _service.openSettings,
        );
      case CameraStatus.unavailable:
        return _buildNoAccess(
          text: 'Камера недоступна на этом устройстве',
          buttonText: 'Повторить',
          onPressed: _init,
        );
    }
  }

  Widget _buildViewfinder() {
    return Stack(
      fit: StackFit.expand,
      children: [
        _service.buildPreview(),

        const Positioned(
          top: 78,
          left: 24,
          right: 24,
          child: Text(
            'Сфотографируйте показания и серийный номер в одном кадре',
            textAlign: TextAlign.center,
            style: TextStyle(
              color: Colors.white,
              fontSize: 18,
              fontWeight: FontWeight.w600,
              shadows: [Shadow(blurRadius: 8, color: Colors.black54)],
            ),
          ),
        ),

        Center(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 16),
            child: AspectRatio(
              aspectRatio: 0.85,
              child: DecoratedBox(
                decoration: BoxDecoration(
                  border: Border.all(color: _accentColor, width: 3),
                  borderRadius: BorderRadius.circular(12),
                ),
              ),
            ),
          ),
        ),

        
        Positioned(
          top: 8,
          left: 8,
          right: 8,
          child: Row(
            children: [
              _roundButton(
                key: const Key('flash_button'),
                icon: _torchOn ? Icons.flashlight_on : Icons.flashlight_off,
                onPressed: _toggleFlash,
              ),
              const Spacer(),
              Material(
                color: Colors.white.withValues(alpha: 0.8),
                shape: const CircleBorder(),
                child: PopupMenuButton<String>(
                  key: const Key('menu_button'),
                  tooltip: 'Меню',
                  icon: const Icon(Icons.more_vert, color: Colors.black87),
                  color: Colors.white,
                  elevation: 8,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                  onSelected: (value) {
                    // Пока заглушка; переход на экран входа подключим позже.
                    _showMessage('Выход пока не реализован.');
                  },
                  itemBuilder: (context) => const [
                    PopupMenuItem<String>(
                      key: Key('menu_exit'),
                      value: 'exit',
                      child: Row(
                        children: [
                          Icon(Icons.logout, color: _errorColor),
                          SizedBox(width: 12),
                          Text('Выход'),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),

        
        Positioned(
          bottom: 24,
          left: 0,
          right: 0,
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              _roundButton(
                key: const Key('switch_button'),
                icon: Icons.flip_camera_android,
                onPressed: _busy ? null : _switchCamera,
              ),
              GestureDetector(
                key: const Key('shutter_button'),
                onTap: _busy ? null : _takePhoto,
                child: Container(
                  width: 72,
                  height: 72,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: Colors.white,
                    border: Border.all(color: Colors.white, width: 4),
                  ),
                  child: Center(
                    child: _busy
                        ? const SizedBox(
                            width: 28,
                            height: 28,
                            child: CircularProgressIndicator(
                              color: _primaryColor,
                              strokeWidth: 3,
                            ),
                          )
                        : const Icon(
                            Icons.camera_alt,
                            color: Colors.black,
                            size: 30,
                          ),
                  ),
                ),
              ),
              _roundButton(
                key: const Key('gallery_button'),
                icon: Icons.image_outlined,
                onPressed: _busy ? null : _openGallery,
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildNoAccess({
    required String text,
    required String buttonText,
    required VoidCallback onPressed,
  }) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(
              Icons.no_photography_outlined,
              color: Colors.white,
              size: 56,
            ),
            const SizedBox(height: 16),
            Text(
              text,
              textAlign: TextAlign.center,
              style: const TextStyle(color: Colors.white, fontSize: 18),
            ),
            const SizedBox(height: 16),
            ElevatedButton(
              key: const Key('permission_button'),
              style: ElevatedButton.styleFrom(
                backgroundColor: _primaryColor,
                foregroundColor: Colors.white,
              ),
              onPressed: onPressed,
              child: Text(buttonText),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPreview(XFile photo) {
    return Stack(
      fit: StackFit.expand,
      children: [
        _service.buildPhoto(photo),
        Positioned(
          left: 24,
          right: 24,
          bottom: 24,
          child: Row(
            children: [
              Expanded(
                child: SizedBox(
                  height: 48,
                  child: OutlinedButton(
                    key: const Key('retake_button'),
                    onPressed: _uploading ? null : _retake,
                    style: OutlinedButton.styleFrom(
                      foregroundColor: Colors.white,
                      backgroundColor: Colors.black38,
                      side: const BorderSide(color: Colors.white),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(8),
                      ),
                    ),
                    child: const Text('Переснять'),
                  ),
                ),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: SizedBox(
                  height: 48,
                  child: ElevatedButton(
                    key: const Key('use_photo_button'),
                    onPressed: _uploading ? null : _usePhoto,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: _primaryColor,
                      foregroundColor: Colors.white,
                      elevation: 0,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(8),
                      ),
                    ),
                    child: _uploading
                        ? const SizedBox(
                            width: 20,
                            height: 20,
                            child: CircularProgressIndicator(
                              key: Key('upload_progress'),
                              color: Colors.white,
                              strokeWidth: 2,
                            ),
                          )
                        : const Text('Использовать'),
                  ),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _roundButton({
    required Key key,
    required IconData icon,
    required VoidCallback? onPressed,
  }) {
    return Material(
      color: Colors.white.withValues(alpha: 0.8),
      shape: const CircleBorder(),
      child: IconButton(
        key: key,
        onPressed: onPressed,
        icon: Icon(icon, color: Colors.black87),
      ),
    );
  }
}
