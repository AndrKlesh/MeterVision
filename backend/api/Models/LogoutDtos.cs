namespace MeterVision.Api.Models;

/// <summary>
/// Represents the data required to log out a user.
/// </summary>
/// <param name="token">The authentication token of the user to invalidate.</param>
public record LogoutDto(string token);