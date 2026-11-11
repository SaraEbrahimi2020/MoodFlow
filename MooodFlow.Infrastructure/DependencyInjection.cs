using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moodflow.Domain.Repositories;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Domain.Repositories;
using MoodFlow.Infrastructure.Repositories;
using MooodFlow.Infrastructure.Authentication;
using MooodFlow.Infrastructure.Repositories;

namespace MoodFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ثبت ریپازیتوری‌ها
        services.AddScoped<IMoodLogRepository, MoodLogRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        // اگر بعداً سرویس‌های دیگری مثل ریپازیتوری‌های دیگر یا کارهای ایمیل و... داشتی هم اینجا ثبت می‌شوند
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}