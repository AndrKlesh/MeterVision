# meter_vision_app

Несколько ресурсов, которые помогут вам начать, если это ваш первый проект на Flutter:

- [Изучение Flutter](https://docs.flutter.dev/get-started/learn-flutter)
- [Обучающие материалы по Flutter](https://docs.flutter.dev/reference/learning-resources)

За помощью в начале разработки на Flutter обратитесь к онлайн-документации, где вы найдёте руководства,
примеры, рекомендации по мобильной разработке и полный справочник по API.

# Слои проекта
---
## Presentation

Папка `presentation` - это слой представления (UI) мобильного приложения.
Здесь живёт всё, что видит и с чем взаимодействует пользователь: экраны,
виджеты, анимации и логика отображения.

Этот слой **не должен** содержать бизнес-логику и работу с данными напрямую (сеть, БД). Для этого есть `data` и `domain` слои.

presentation/
├── views/  **Экраны приложения (страницы)**
└── widgets/ **Переиспользуемые UI-компоненты**

**Именование (Папки -> Класса):**
   - Экраны: `*_view.dart` → `CameraView`, `LoginView`
   - Виджеты: `имя_компонента.dart` → `PrimaryButton`

### `views/` — Экраны

Здесь хранятся полноценные экраны приложения - то, что пользователь
открывает через навигацию.

**Структура:**
views/
├── camera/
│ └── camera_view.dart
├── login/
│ └── login_view.dart
└── settings/
└── settings_view.dart

## `widgets/` — Виджеты

Здесь находятся **переиспользуемые UI-компоненты**, которые используются
в нескольких экранах или внутри одного экрана несколько раз:
- Кнопки, поля ввода, карточки, диалоги, лоадеры
- Мелкие составные блоки (например, `ProductTile`)
- Виджеты, которые не зависят от конкретного экрана

**Структура:**
widgets/
├── buttons/
│ ├── primary_button.dart
│ └── icon_button.dart
├── inputs/
│ └── password_text_field.dart
├── app_bar/
│ └── custom_app_bar.dart
└── loaders/
└── app_loader.dart


---

## Core

Папка `core` - это **фундамент приложения**. Здесь лежит всё, что используется во всех слоях и не относится к конкретной фиче:
тема, строки, константы, форматы дат, расщирения и т.д.

> 💡 Правило: если что-то нужно **больше чем в одном месте** и не является
> частью конкретного экрана или фичи - скорее всего, это `core`.

core/
├── theme/ **Тема приложения (ThemeData), цвета, отступы, радиусы**
├── strings/ **Все тексты приложения**
└── assets/ **Пути к папкам**


## `theme/` — Темы, стили

Папка `theme` - это хранилище всего, что отвечает за внешний вид приложения:
цвета, шрифты, отступы, скругления, тени. Она находится внутри core.

core/theme/
├── app_theme.dart   **Сборка общей темы (ThemeData)**
├── app_colors.dart  **Палитра цветов**
└── app_sizes.dart   **Отступы, радиусы, размеры**

**Пример:**
```dart
// theme/app_colors.dart
class AppColors {
  AppColors._();

  static const Color primary = Color(0xFF2196F3);
  static const Color secondary = Color(0xFF03DAC6);
  static const Color background = Color(0xFFF5F5F5);
  static const Color textPrimary = Color(0xFF212121);
  static const Color textSecondary = Color(0xFF757575);
  static const Color error = Color(0xFFB00020);
}

// theme/app_sizes.dart
class AppSizes {
  AppSizes._();

  static const double paddingXS = 4;
  static const double paddingS  = 8;
  static const double paddingM  = 16;
  static const double paddingL  = 24;
  static const double radiusM   = 12;
}
```

**Использование:**
```dart
Container(
  padding: const EdgeInsets.all(AppSizes.paddingM),
  decoration: BoxDecoration(
    color: AppColors.primary,
  ),
)
```

## `strings/` — Строки приложения

Здесь хранятся все тексты приложения в одном месте. Это нужно для того, чтобы легко менять текст без поиска строк по всему проекту.

**Пример:**
```dart
// strings/app_strings.dart
class AppStrings {
  AppStrings._();

  static const String appName = 'My App';

  // Логин
  static const String loginTitle = 'Вход';
  static const String loginButton = 'Войти';
  static const String emailHint = 'Логин';
  static const String passwordHint = 'Пароль';

  // Камера
  static const String cameraTitle = 'Камера';
  static const String cameraHint = 'Наведите камеру ...';

  // Ошибки
  static const String errorNetwork = 'Проверьте подключение к интернету';
  static const String errorIncorrectLoginOrPassword = 'Неправильный логин или пароль';
}
```

**Использование:**
```dart
Text(AppStrings.loginTitle)
```

## `assets/` — Ресурсы (изображения, иконки, шрифты)

Здесь хранятся **пути к файлам ресурсов** (сами файлы — в `assets/`
в корне проекта). Это нужно, чтобы не хардкодить пути в виджетах.

**Структура:**
core/assets/
├── app_images.dart **Пути к изображениям**
└── app_fonts.dart **Названия шрифтовых семейств**

**Пример:**
```dart
class AppImages {
  AppImages._();

  static const String _basePath = 'assets/images/';

  static const String logo = '${_basePath}logo.png';
  static const String emptyState = '${_basePath}empty_state.png';
}
```

**Использование:**
```dart
Image(image: AssetImage(AppImages.flutter))
```


## Assets
Папка `assets` — это **хранилище всех статических ресурсов** приложения:
изображений, иконок, шрифтов, анимаций и других файлов, которые
не являются кодом.

> 💡 Простыми словами: здесь лежат «картинки и шрифты», которые
> приложение показывает пользователю.

**Структура:**
assets/ **Физические файлы**
├── fonts/
│   ├── Roboto-Regular.ttf
│   ├── Roboto-Medium.ttf
│   └── Roboto-Bold.ttf
├── images/
    └── ic_flutter.png