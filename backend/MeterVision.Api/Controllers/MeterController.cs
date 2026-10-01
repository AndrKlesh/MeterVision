using Microsoft.AspNetCore.Mvc;
using MeterVision.Api.Models;

namespace MeterVision.Api.Controllers
{
    [Route("api/meters")]
    [ApiController]
    public class MetersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MetersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMeter([FromBody] MeterDataDto data)
        {
            var client = _httpClientFactory.CreateClient();

            // Адрес сервиса вашего коллеги для проверки существования пользователя
            string colleagueCheckUrl = $"http://localhost:5001/api/users/exists?username={data.Username}";

            try
            {
                // Заглушка: пока считаем, что коллега подтвердил наличие пользователя
                bool userExistsFromColleague = true; 

                if (!userExistsFromColleague)
                {
                    return BadRequest(new { success = false, message = "Ошибка: такого пользователя нет." });
                }

                Console.WriteLine($" Пользователь {data.Username} подтвержден. Значение: {data.MeterValue}");

                return Ok(new 
                { 
                    success = true, 
                    message = "Показания приняты, пользователь подтвержден сервисом коллеги!",
                    receivedData = data 
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Не удалось связаться с сервисом проверки пользователей (коллеги)." });
            }
        }
    }
}