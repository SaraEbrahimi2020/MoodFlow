using Microsoft.AspNetCore.Mvc;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Application.Common.Models;

namespace MoodFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
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
        var (isSuccess, token, errors) = await _identityService.LoginAsync(
            request.Email,
            request.Password);

        if (!isSuccess)
        {
            return Unauthorized(new { Errors = errors });
        }

        return Ok(new AuthResponse { Token = token });
    }
}