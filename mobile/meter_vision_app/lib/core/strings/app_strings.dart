class AppStrings {
  AppStrings._();

  static const String appName = 'MeterVision';

  static const String cameraHint ='Сфотографируйте показания и серийный номер в одном кадре';
  static const String cameraMenuTooltip = 'Меню';
  static const String cameraMenuExit = 'Выход';
  static const String cameraRetake = 'Переснять';
  static const String cameraUsePhoto = 'Использовать';
  static const String cameraGotIt = 'Понятно';

 
  static const String cameraPermissionDenied = 'Разрешите доступ к камере, чтобы сфотографировать счётчик';
  static const String cameraPermissionAllow = 'Разрешить';
  static const String cameraPermissionPermanentlyDenied = 'Доступ к камере запрещён. Разрешите его в настройках';
  static const String cameraOpenSettings = 'Открыть настройки';
  static const String cameraUnavailable = 'Камера недоступна на этом устройстве';
  static const String cameraRetry = 'Повторить';


  static const String errorTorchUnavailable =  'Фонарик недоступен на этом устройстве.';
  static const String errorSwitchCamera = 'Не удалось переключить камеру.';
  static const String errorTakePhoto = 'Не удалось сделать снимок. Попробуйте ещё раз.';
  static const String errorOpenGallery = 'Не удалось открыть галерею.';
  static const String errorCameraNotStarted = 'Камера не запущена';

  
  static const String errorPhotoFormat = 'Неподдерживаемый формат. Допустимы JPG, PNG, HEIC.';
  static const String errorPhotoEmpty ='Снимок пустой. Сфотографируйте ещё раз.';
  static const String errorPhotoTooLarge ='Снимок слишком большой. Максимум 10 МБ.';

  
  static const String stubUploadNotImplemented = 'Отправка фото пока не реализована.';
  static const String stubExitNotImplemented = 'Выход пока не реализован.';
}
