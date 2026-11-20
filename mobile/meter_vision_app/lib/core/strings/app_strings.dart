class AppStrings {
  AppStrings._();

  static const String appName = 'MeterVision';

  //
  static const String loginTitle = 'Вход в аккаунт';
  static const String loginSubtitle = 'Введите свои данные для продолжения';

  static const String emailLabel = 'E-mail';
  static const String emailHint = 'Введите адрес эл. почты';

  static const String passwordLabel = 'Пароль';
  static const String passwordHint = 'Введите пароль';

  static const String loginButton = 'Войти';

  static const String errorInvalidEmail = 'Адрес эл. почты неверен';
  static const String errorEmptyEmail = 'Введите почту';
  static const String errorEmptyPassword = 'Введите пароль';

  static const String cancel = 'Отменить';
  static const String retry = 'Повторить';

  static const String showPassword = 'Показать пароль';
  static const String hidePassword = 'Скрыть пароль';

  static const String cameraHint =
      'Сфотографируйте показания и серийный номер в одном кадре';
  static const String cameraMenuTooltip = 'Меню';
  static const String cameraMenuExit = 'Выход';
  static const String cameraRetake = 'Переснять';
  static const String cameraUsePhoto = 'Использовать';
  static const String cameraGotIt = 'Понятно';

  static const String cameraPermissionDenied =
      'Разрешите доступ к камере, чтобы сфотографировать счётчик';
  static const String cameraPermissionAllow = 'Разрешить';
  static const String cameraPermissionPermanentlyDenied =
      'Доступ к камере запрещён. Разрешите его в настройках';
  static const String cameraOpenSettings = 'Открыть настройки';
  static const String cameraUnavailable =
      'Камера недоступна на этом устройстве';
  static const String cameraRetry = 'Повторить';

  static const String errorTorchUnavailable =
      'Фонарик недоступен на этом устройстве.';
  static const String errorSwitchCamera = 'Не удалось переключить камеру.';
  static const String errorTakePhoto =
      'Не удалось сделать снимок. Попробуйте ещё раз.';
  static const String errorOpenGallery = 'Не удалось открыть галерею.';
  static const String errorCameraNotStarted = 'Камера не запущена';

  static const String errorPhotoFormat =
      'Неподдерживаемый формат. Допустимы JPG, PNG, HEIC.';
  static const String errorPhotoEmpty =
      'Снимок пустой. Сфотографируйте ещё раз.';
  static const String errorPhotoTooLarge =
      'Снимок слишком большой. Максимум 10 МБ.';

  static const String stubUploadNotImplemented =
      'Отправка фото пока не реализована.';
  static const String stubExitNotImplemented = 'Выход пока не реализован.';

  static const String scanResultTitle = 'Результаты распознавания';
  static const String takePhoto = 'Сделать снимок';
  static const String submitFailed =
      'Не удалось отправить показания. Попробуйте ещё раз.';
  static const String emptyReading = 'Введите показания';
  static const String emptySerial = 'Введите серийный номер';
  static const String shortSerial = 'Серийный номер слишком короткий';
  static const String readingsSent = 'Показания отправлены';
  static const String readingLabel = 'Показания';
  static const String serialLabel = 'Серийный номер';
  static const String sending = 'Отправляем…';
  static const String submitReadings = 'Отправить показания';
  static const String confidenceTitle = 'Уверенность распознавания';
  static const String confidenceHigh = 'Высокая уверенность распознавания';
  static const String confidenceMedium = 'Средняя уверенность распознавания';
  static const String confidenceLow =
      'Низкая уверенность распознавания. Проверьте правильность значений.';
  static const String confidenceCritical =
      'Критически низкая уверенность. Требуется ручная проверка.';
  static const String invalidCredentials = 'Неправильный логин или пароль';
  static const String emptyPhoto = 'Пустой снимок';
  static const String unknownPhoto = 'Неизвестный снимок';
  static const String recognitionFailed =
      'Не удалось распознать фото. Повторите отправку или переснимите счётчик.';
  static const String close = 'Закрыть';
  static const String logoutFailed = 'Не удалось выйти. Попробуйте ещё раз.';
  static const String choosePhoto = 'Выбрать фото из галереи';
  static const String counterPrompt =
      'You have pushed the button this many times:';
  static const String increment = 'Increment';
  static String percentage(int value) => '$value%';
}
