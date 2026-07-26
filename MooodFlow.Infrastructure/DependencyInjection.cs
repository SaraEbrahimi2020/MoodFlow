using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoodFlow.Domain.Repositories;
using MoodFlow.Infrastructure.Repositories;

namespace MoodFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ثبت ریپازیتوری‌ها
        services.AddScoped<IMoodLogRepository, MoodLogRepository>();

        // اگر بعداً سرویس‌های دیگری مثل ریپازیتوری‌های دیگر یا کارهای ایمیل و... داشتی هم اینجا ثبت می‌شوند

        return services;
    }
}