using MediatR;
using Moodflow.Domain.Repositories;
using MoodFlow.Application.Common.Interfaces;

namespace MoodFlow.Application.Features.Auth.Commands;

public record LogoutCommand() : IRequest<Unit>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ICurrentUserService _currentUserService;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ICurrentUserService currentUserService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // ۱. گرفتن شناسه کاربر جاری از توکن
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }

        // ۲. دریافت تمام توکن‌های فعال کاربر از طریق ریپازیتوری
        var activeTokens = await _refreshTokenRepository.GetActiveTokensByUserIdAsync(userId, cancellationToken);

        // ۳. باطل کردن تمام توکن‌ها در حافظه
        foreach (var token in activeTokens)
        {
            token.Revoke();
        }

        // ۴. ذخیره‌سازی قطعی تغییرات در دیتابیس
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}