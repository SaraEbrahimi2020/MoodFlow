using System.Security.Claims;

namespace MoodFlow.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateRefreshToken();
    string GenerateToken(string userId, string email, string userName);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
