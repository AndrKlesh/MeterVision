import 'dart:io';

import 'package:camera/camera.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:image_picker/image_picker.dart';
import 'package:meter_vision_app/core/assets/app_images.dart';
import 'package:meter_vision_app/core/strings/app_strings.dart';
import 'package:meter_vision_app/core/theme/app_colors.dart';
import 'package:meter_vision_app/core/theme/app_sizes.dart';
import 'package:meter_vision_app/presentation/widgets/buttons/svg_button.dart';
import 'package:meter_vision_app/presentation/widgets/dialogs/error_bottom_sheet.dart';
import 'package:meter_vision_app/presentation/widgets/overlays/scanner_frame.dart';
import 'package:permission_handler/permission_handler.dart';

const List<String> _allowedExtensions = ['jpg', 'jpeg', 'png', 'heic', 'heif'];


enum _CameraStatus {
  loading,
  ready,
  permissionDenied,
  permissionPermanentlyDenied,
  unavailable,
}


String? validatePhoto(String path, int size) {
  final dot = path.lastIndexOf('.');
  final ext = dot == -1 ? '' : path.substring(dot + 1).toLowerCase();
  if (!_allowedExtensions.contains(ext)) return AppStrings.errorPhotoFormat;
  if (size == 0) return AppStrings.errorPhotoEmpty;
  if (size > AppSizes.maxPhotoBytes) return AppStrings.errorPhotoTooLarge;
  return null;
}


class CameraView extends StatefulWidget {
  const CameraView({super.key});

  @override
  State<CameraView> createState() => _CameraViewState();
}

class _CameraViewState extends State<CameraView> {
  CameraController? _controller;
  List<CameraDescription> _cameras = [];

  _CameraStatus _status = _CameraStatus.loading;
  bool _torchOn = false;
  bool _busy = false;
  bool _uploading = false;
  XFile? _captured;

  @override
  void initState() {
    super.initState();
    _init();
  }

  @override
  void dispose() {
    _controller?.dispose();
    super.dispose();
  }

  // ---------- Камера ----------

  Future<void> _init() async {
    setState(() => _status = _CameraStatus.loading);
    final status = await _initCamera();
    if (!mounted) return;
    setState(() => _status = status);
  }

  Future<_CameraStatus> _initCamera() async {
    final permission = await Permission.camera.request();
    if (permission.isPermanentlyDenied) {
      return _CameraStatus.permissionPermanentlyDenied;
    }
    if (!permission.isGranted) return _CameraStatus.permissionDenied;

    try {
      _cameras = await availableCameras();
      if (_cameras.isEmpty) return _CameraStatus.unavailable;

      final back = _cameras.firstWhere(
        (c) => c.lensDirection == CameraLensDirection.back,
        orElse: () => _cameras.first,
      );
      await _startCamera(back);
      return _CameraStatus.ready;
    } catch (_) {
      return _CameraStatus.unavailable;
    }
  }

  Future<void> _startCamera(CameraDescription description) async {
    final old = _controller;
    if (old != null) {
      
      setState(() => _controller = null);
      await old.dispose();
    }

    final controller = CameraController(
      description,
      ResolutionPreset.high,
      enableAudio: false,
    );
    await controller.initialize();

    if (!mounted) {
      await controller.dispose();
      return;
    }
    setState(() {
      _controller = controller;
      _torchOn = false;
    });
  }

  Future<void> _toggleFlash() async {
    final next = !_torchOn;
    try {
      await _controller?.setFlashMode(next ? FlashMode.torch : FlashMode.off);
      if (mounted) setState(() => _torchOn = next);
    } catch (_) {
      await _showMessage(AppStrings.errorTorchUnavailable);
    }
  }

  Future<void> _switchCamera() async {
    if (_busy) return;
    final current = _controller?.description;
    if (current == null || _cameras.length < 2) return;
    try {
      final next = _cameras.firstWhere(
        (c) => c.lensDirection != current.lensDirection,
        orElse: () => current,
      );
      await _startCamera(next);
    } catch (_) {
      await _showMessage(AppStrings.errorSwitchCamera);
    }
  }

  Future<void> _takePhoto() async {
    if (_busy || _status != _CameraStatus.ready) return;
    setState(() => _busy = true);
    try {
      final controller = _controller;
      if (controller == null) throw StateError(AppStrings.errorCameraNotStarted);
      final photo = await controller.takePicture();
      await _checkAndKeep(photo);
    } catch (_) {
      await _showMessage(AppStrings.errorTakePhoto);
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _openGallery() async {
    if (_busy) return;
    setState(() => _busy = true);
    try {
      final photo = await ImagePicker().pickImage(source: ImageSource.gallery);
      if (photo != null) await _checkAndKeep(photo);
    } catch (_) {
      await _showMessage(AppStrings.errorOpenGallery);
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _checkAndKeep(XFile photo) async {
    final size = await photo.length();
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
    if (_uploading || _captured == null) return;

    setState(() => _uploading = true);
    try {
      // Временная имитация отправки: сервер пока не подключён.
      await Future<void>.delayed(const Duration(milliseconds: 700));
    } finally {
      if (mounted) setState(() => _uploading = false);
    }

    await _showMessage(AppStrings.stubUploadNotImplemented);
  }

  Future<void> _showMessage(String message) async {
    if (!mounted) return;
    await showErrorBottomSheet(context, message);
  }

  // ---------- UI ----------

  @override
  Widget build(BuildContext context) {
    final captured = _captured;
    return Scaffold(
      backgroundColor: Colors.black,
      body: SafeArea(
        child: captured != null ? _buildPhotoPreview(captured) : _buildBody(),
      ),
    );
  }

  Widget _buildBody() {
    switch (_status) {
      case _CameraStatus.loading:
        return const Center(
          child: CircularProgressIndicator(color: Colors.white),
        );
      case _CameraStatus.ready:
        return _buildViewfinder();
      case _CameraStatus.permissionDenied:
        return _buildNoAccess(
          text: AppStrings.cameraPermissionDenied,
          buttonText: AppStrings.cameraPermissionAllow,
          onPressed: _init,
        );
      case _CameraStatus.permissionPermanentlyDenied:
        return _buildNoAccess(
          text: AppStrings.cameraPermissionPermanentlyDenied,
          buttonText: AppStrings.cameraOpenSettings,
          onPressed: openAppSettings,
        );
      case _CameraStatus.unavailable:
        return _buildNoAccess(
          text: AppStrings.cameraUnavailable,
          buttonText: AppStrings.cameraRetry,
          onPressed: _init,
        );
    }
  }

  Widget _buildCameraPreview() {
    final controller = _controller;
    if (controller == null || !controller.value.isInitialized) {
      return const ColoredBox(color: AppColors.cameraPlaceholder);
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

  Widget _buildViewfinder() {
    return Stack(
      fit: StackFit.expand,
      children: [
        _buildCameraPreview(),
        const ScannerFrame(),

        const Positioned(
          top: AppSizes.hintTop,
          left: AppSizes.hintHorizontal,
          right: AppSizes.hintHorizontal,
          child: Text(
            AppStrings.cameraHint,
            textAlign: TextAlign.center,
            style: TextStyle(
              color: Colors.white,
              fontSize: AppSizes.hintFontSize,
              height: AppSizes.hintLineHeight,
              fontWeight: FontWeight.w600,
            ),
          ),
        ),


        Positioned(
          top: AppSizes.paddingS,
          left: AppSizes.paddingS,
          right: AppSizes.paddingS,
          child: Row(
            children: [
              SvgButton(
                key: const Key('flash_button'),
                asset: AppImages.flashlight,
                size: AppSizes.smallButtonSize,
                onPressed: _toggleFlash,
              ),
              const Spacer(),
              PopupMenuButton<String>(
                key: const Key('menu_button'),
                tooltip: AppStrings.cameraMenuTooltip,
                color: Colors.white,
                elevation: 8,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(AppSizes.radiusM),
                ),
                onSelected: (_) {
                  // Пока заглушка; переход на экран входа подключим позже.
                  _showMessage(AppStrings.stubExitNotImplemented);
                },
                itemBuilder: (context) => const [
                  PopupMenuItem<String>(
                    key: Key('menu_exit'),
                    value: 'exit',
                    child: Row(
                      children: [
                        Icon(Icons.logout, color: AppColors.error),
                        SizedBox(width: AppSizes.radiusM),
                        Text(AppStrings.cameraMenuExit),
                      ],
                    ),
                  ),
                ],
                child: SvgPicture.asset(
                  AppImages.menu,
                  width: AppSizes.smallButtonSize,
                  height: AppSizes.smallButtonSize,
                ),
              ),
            ],
          ),
        ),

        Positioned(
          bottom: AppSizes.paddingL,
          left: 0,
          right: 0,
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              SvgButton(
                key: const Key('switch_button'),
                asset: AppImages.switchCamera,
                onPressed: _busy ? null : _switchCamera,
              ),
              GestureDetector(
                key: const Key('shutter_button'),
                behavior: HitTestBehavior.opaque,
                onTap: _busy ? null : _takePhoto,
                child: SizedBox(
                  width: AppSizes.captureButtonSize,
                  height: AppSizes.captureButtonSize,
                  child: Stack(
                    alignment: Alignment.center,
                    children: [
                      SvgPicture.asset(
                        AppImages.capture,
                        width: AppSizes.captureButtonSize,
                        height: AppSizes.captureButtonSize,
                      ),
                      if (_busy)
                        const SizedBox(
                          width: AppSizes.shutterLoaderSize,
                          height: AppSizes.shutterLoaderSize,
                          child: CircularProgressIndicator(
                            color: AppColors.primary,
                            strokeWidth: 3,
                          ),
                        ),
                    ],
                  ),
                ),
              ),
              SvgButton(
                key: const Key('gallery_button'),
                asset: AppImages.gallery,
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
        padding: const EdgeInsets.all(AppSizes.paddingL),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(
              Icons.no_photography_outlined,
              color: Colors.white,
              size: AppSizes.noAccessIconSize,
            ),
            const SizedBox(height: AppSizes.paddingM),
            Text(
              text,
              textAlign: TextAlign.center,
              style: const TextStyle(color: Colors.white, fontSize: 18),
            ),
            const SizedBox(height: AppSizes.paddingM),
            ElevatedButton(
              key: const Key('permission_button'),
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.primary,
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

  Widget _buildPhotoPreview(XFile photo) {
    return Stack(
      fit: StackFit.expand,
      children: [
        Image.file(File(photo.path), fit: BoxFit.cover),
        Positioned(
          left: AppSizes.paddingL,
          right: AppSizes.paddingL,
          bottom: AppSizes.paddingL,
          child: Row(
            children: [
              Expanded(
                child: SizedBox(
                  height: AppSizes.buttonHeight,
                  child: OutlinedButton(
                    key: const Key('retake_button'),
                    onPressed: _uploading ? null : _retake,
                    style: OutlinedButton.styleFrom(
                      foregroundColor: Colors.white,
                      backgroundColor: Colors.black38,
                      side: const BorderSide(color: Colors.white),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(AppSizes.radiusS),
                      ),
                    ),
                    child: const Text(AppStrings.cameraRetake),
                  ),
                ),
              ),
              const SizedBox(width: AppSizes.radiusM),
              Expanded(
                child: SizedBox(
                  height: AppSizes.buttonHeight,
                  child: ElevatedButton(
                    key: const Key('use_photo_button'),
                    onPressed: _uploading ? null : _usePhoto,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: AppColors.primary,
                      foregroundColor: Colors.white,
                      elevation: 0,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(AppSizes.radiusS),
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
                        : const Text(AppStrings.cameraUsePhoto),
                  ),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}