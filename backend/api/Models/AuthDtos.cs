namespace MeterVision.Api.Models;

public record LoginDto(string Username, string Password);
public record LogoutDto(string token);