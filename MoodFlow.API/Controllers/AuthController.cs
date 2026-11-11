using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Application.Common.Models;
using MoodFlow.Application.Features.Auth.Commands;

namespace MoodFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IMediator _mediator;

    public AuthController(IIdentityService identityService, IMediator mediator)
    {
        _identityService = identityService;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (isSuccess, userId, errors) = await _identityService.RegisterUserAsync(
            request.Email,
            request.Password,
            request.UserName);

        if (!isSuccess)
        {
            return BadRequest(new { Errors = errors });
        }

        return Ok(new { Message = "ثبت‌نام با موفقیت انجام شد.", UserId = userId });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // دریافت ۴ متغیر متناظر با خروجی جدید LoginAsync
        var (isSuccess, token, refreshToken, errors) = await _identityService.LoginAsync(
            request.Email,
            request.Password);

        if (!isSuccess)
        {
            return Unauthorized(new { Errors = errors });
        }

        return Ok(new AuthResponse
        (
           token,
           refreshToken,
           DateTime.UtcNow.AddHours(8) // مطابق با AddHours(8) در JwtTokenGenerator شما
        ));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize] // فقط کاربر واردشده می‌تواند لاگ‌اوت کند
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return Ok(new { message = "با موفقیت از حساب کاربری خارج شدید." });
    }
}