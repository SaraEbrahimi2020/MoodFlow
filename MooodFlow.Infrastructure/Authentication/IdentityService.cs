using Microsoft.AspNetCore.Identity;
using MoodFlow.Application.Common.Interfaces;

namespace MooodFlow.Infrastructure.Authentication;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public IdentityService(
        UserManager<IdentityUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    // ۱. متد ثبت‌نام کاربر جدید
    public async Task<(bool IsSuccess, string UserId, IEnumerable<string> Errors)> RegisterUserAsync(
        string email,
        string password,
        string userName)
    {
        var user = new IdentityUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return (true, user.Id, Enumerable.Empty<string>());
        }

        var errors = result.Errors.Select(e => e.Description);
        return (false, string.Empty, errors);
    }

    // ۲. متد لاگین و دریافت توکن JWT
    public async Task<(bool IsSuccess, string Token, IEnumerable<string> Errors)> LoginAsync(
        string email,
        string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return (false, string.Empty, new[] { "کاربری با این ایمیل یافت نشد." });
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return (false, string.Empty, new[] { "رمز عبور اشتباه است." });
        }

        // تولید توکن JWT در صورت صحیح بودن اطلاعات
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, user.UserName!);

        return (true, token, Enumerable.Empty<string>());
    }
}