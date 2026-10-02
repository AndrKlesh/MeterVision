using Microsoft.AspNetCore.Mvc;
using MeterVision.Api.Models;

namespace MeterVision.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        // 1. Вход в систему (POST /api/auth/login)
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            // Заглушка: проверяем логин и пароль
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ApiResponseDto 
                { 
                    Success = false, 
                    Message = "Имя пользователя и пароль обязательны." 
                });
            }

            // Генерируем случайный уникальный ID сессии (Guid)
            string generatedSessionId = Guid.NewGuid().ToString();

            var response = new LoginResponseDto
            {
                Success = true,
                SessionId = generatedSessionId,
                Message = "Успешный вход в систему!"
            };

            return Ok(response);
        }

        // 2. Выход из текущей сессии (POST /api/auth/logout)
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequestDto request)
        {
            if (string.IsNullOrEmpty(request.SessionId))
            {
                return BadRequest(new ApiResponseDto 
                { 
                    Success = false, 
                    Message = "Идентификатор сессии не передан." 
                });
            }

            // Заглушка: успешное удаление сессии
            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = $"Сессия {request.SessionId} успешно завершена."
            });
        }

        // 3. Выход со всех устройств (POST /api/auth/logout-all)
        [HttpPost("logout-all")]
        public IActionResult LogoutAll([FromBody] LogoutAllRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new ApiResponseDto 
                { 
                    Success = false, 
                    Message = "Имя пользователя не передано." 
                });
            }

            // Заглушка: успешное удаление всех сессий пользователя
            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = $"Все сессии для пользователя '{request.Username}' успешно сброшены."
            });
        }
    }
}