namespace MeterVision.Api.Models
{
    // Класс для счетчиков (нужен для MetersController)
    public class MeterDataDto
    {
        public string Username { get; set; } = string.Empty;
        public double MeterValue { get; set; }
        public string ImageInfo { get; set; } = string.Empty;
    }

    // Запрос на вход
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Ответ при успешном входе
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // Запрос на выход из текущей сессии
    public class LogoutRequestDto
    {
        public string SessionId { get; set; } = string.Empty;
    }

    // Запрос на выход со всех устройств
    public class LogoutAllRequestDto
    {
        public string Username { get; set; } = string.Empty;
    }

    // Общий ответ для операций выхода
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}