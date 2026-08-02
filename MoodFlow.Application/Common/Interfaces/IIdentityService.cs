namespace MoodFlow.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool IsSuccess, string UserId, IEnumerable<string> Errors)> RegisterUserAsync(string email, string password, string userName);
    Task<(bool IsSuccess, string Token, IEnumerable<string> Errors)> LoginAsync(string email, string password);
}
