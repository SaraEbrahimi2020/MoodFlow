
namespace MoodFlow.Application.Common.Models;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt
);