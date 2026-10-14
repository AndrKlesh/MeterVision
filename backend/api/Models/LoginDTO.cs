
namespace MeterVision.Api.Models;

/// <summary>
/// Represents the data required to authenticate a user.
/// </summary>
/// <param name="Username">The username of the user.</param>
/// <param name="Password">The password of the user.</param>
public record LoginDto(string Username, string Password);
