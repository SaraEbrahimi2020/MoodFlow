using MediatR;
using Moodflow.Domain.Entities;
using Moodflow.Domain.Repositories;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Application.Common.Models;
using System.Security.Claims;

namespace MoodFlow.Application.Features.Auth.Commands;

public record RefreshTokenCommand(
    string Token,
    string RefreshToken
) : IRequest<AuthResponse>;
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // ۱. استخراج Claims از AccessToken منقضی شده
        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.Token);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? principal.FindFirst("sub")?.Value;
        var email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        var userName = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("توکن ورودی غیرمعتبر است.");
        }

        // ۲. پیدا کردن Refresh Token از طریق ریپازیتوری
        var existingRefreshToken = await _refreshTokenRepository
            .GetByTokenAndUserIdAsync(request.RefreshToken, userId, cancellationToken);

        // ۳. اعتبارسنجی توکن (وجود، انقضا و فعال بودن)
        if (existingRefreshToken == null || !existingRefreshToken.IsActive)
        {
            throw new UnauthorizedAccessException("توکن نوسازی نامعتبر، منقضی یا باطل شده است.");
        }

        // ۴. علامت‌گذاری توکن قبلی به عنوان استفاده شده
        existingRefreshToken.MarkAsUsed();

        // ۵. مشخص کردن تاریخ انقضای Access Token جدید (مثلاً ۱۵ دقیقه بعد)
        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);

        // ۶. تولید Access Token جدید (با ارسال userId, email, userName) و Refresh Token جدید
        var newAccessToken = _jwtTokenGenerator.GenerateToken(userId, email, userName);
        var newRefreshTokenString = _jwtTokenGenerator.GenerateRefreshToken();

        // ۷. ساخت انتیتی جدید و اضافه کردن به ریپازیتوری
        var newRefreshToken = new RefreshToken
        {
            UserId = userId,
            Token = newRefreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(7) // طول عمر ۷ روزه برای رفرش توکن
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        // ۸. ذخیره قطعی تغییرات در دیتابیس
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        // ۹. بازگرداندن پاسخ تطبیق داده‌شده با AuthResponse شما
        return new AuthResponse(
            newAccessToken,
            newRefreshTokenString,
            accessTokenExpiresAt
        );
    }
}