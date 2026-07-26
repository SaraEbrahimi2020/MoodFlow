using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using System.Reflection;
using MoodFlow.Application.Common.Behaviors; // آدرس دقیق پوشه بیهیویر خودت را بگذار

namespace MoodFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // گرفتن اسمبلی جاری لایه Application برای اسکن خودکار
        var assembly = Assembly.GetExecutingAssembly();

        // ۱. ثبت و راه‌اندازی MediatR به همراه پایپ‌لاین‌های مربوطه
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);

            // اضافه کردن پلیس راهور ولیدیشن سر راه تمام ترتزکشن‌های مدیاآر
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // ۲. ثبت خودکار تمام Validatorهای لایه Application از طریق اسکن اسمبلی
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}