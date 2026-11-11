using FluentAssertions;
using Moodflow.Domain.Entities;
using Moodflow.Domain.Repositories;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Application.Features.Auth.Commands;
using MoodFlow.Domain.Repositories;
using Moq;
using System.Security.Claims;
using Xunit;

namespace MoodFlow.Application.Tests.Features.Auth;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly RefreshTokenCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
    {
        // ۱. ساخت اشیاء شبیه‌سازی‌شده (Mock)
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        // ۲. تزریق Mockها به هندلر اصلی
        _handler = new RefreshTokenCommandHandler(
            _refreshTokenRepositoryMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsInvalid_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange (آماده‌سازی دیتای تست)
        var command = new RefreshTokenCommand("expired_access_token", "invalid_refresh_token");

        // شبیه‌سازی رفتار JWT Generator برای استخراج Claim
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user-123"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Name, "testuser")
        }));

        _jwtTokenGeneratorMock
            .Setup(x => x.GetPrincipalFromExpiredToken(command.Token))
            .Returns(claimsPrincipal);

        // شبیه‌سازی پیدا نشدن توکن در دیتابیس (null برمی‌گرداند)
        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAndUserIdAsync(command.RefreshToken, "user-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshToken?)null);

        // Act & Assert (اجرا و بررسی نتیجه)
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("توکن نوسازی نامعتبر، منقضی یا باطل شده است.");
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsValid_ShouldReturnNewTokensAndRevokeOldOne()
    {
        // Arrange (آماده‌سازی دیتای تست)
        var command = new RefreshTokenCommand("valid_access_token", "valid_refresh_token");

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, "user-123"),
        new Claim(ClaimTypes.Email, "test@example.com"),
        new Claim(ClaimTypes.Name, "testuser")
    }, "TestAuth"));

        // ۱. شبیه‌سازی استخراج Claimها از توکن منقضی شده
        _jwtTokenGeneratorMock
            .Setup(x => x.GetPrincipalFromExpiredToken(command.Token))
            .Returns(claimsPrincipal);

        // ۲. ساخت یک نمونه Refresh Token معتبر موجود در دیتابیس
        var existingRefreshToken = new RefreshToken
        {
            Id = 1,
            UserId = "user-123",
            Token = "valid_refresh_token",
            ExpiresAt = DateTime.UtcNow.AddDays(5),
            IsRevoked = false,
            IsUsed = false
        };

        // ۳. شبیه‌سازی پیدا شدن توکن در ریپازیتوری
        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAndUserIdAsync(command.RefreshToken, "user-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingRefreshToken);

        // ۴. شبیه‌سازی تولید توکن‌های جدید توسط Generator
        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken("user-123", "test@example.com", "testuser"))
            .Returns("new_access_token_string");

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns("new_refresh_token_string");

        // Act (اجرای هندلر)
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert (بررسی صحت نتایج)
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new_access_token_string");
        result.RefreshToken.Should().Be("new_refresh_token_string");

        // بررسی اینکه توکن قبلی در دیتابیس به عنوان مصرف‌شده/باطل‌شده علامت‌گذاری شده باشد
        existingRefreshToken.IsUsed.Should().BeTrue();
       // existingRefreshToken.IsRevoked.Should().BeTrue();

        // بررسی اینکه متدهای SaveChanges و Add ریپازیتوری فراخوانی شده باشند
        _refreshTokenRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        _refreshTokenRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}