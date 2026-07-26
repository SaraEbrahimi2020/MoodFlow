using FluentValidation;
using MoodFlow.Application.Features.MoodLogs.Commands;

namespace MoodFlow.Application.MoodLogs.Validators;

public class CreateMoodLogCommandValidator : AbstractValidator<CreateMoodLogCommand>
{
    public CreateMoodLogCommandValidator()
    {
        // ۱. اعتبارسنجی میزان انرژی (باید بین ۱ تا ۵ باشد)
        RuleFor(x => x.EnergyLevel)
            .InclusiveBetween(1, 5)
            .WithMessage("میزان انرژی باید عددی بین ۱ تا ۵ باشد.");

        // ۲. اعتبارسنجی فیلد شکرگزاری (نباید خالی باشد و حداکثر ۲۰۰ کاراکتر)
        RuleFor(x => x.Gratitude1)
            .NotEmpty().WithMessage("لطفاً حداقل یک مورد شکرگزاری صبحگاهی را وارد کنید.")
            .MaximumLength(200).WithMessage("متن شکرگزاری نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.");

        // ۳. اعتبارسنجی جملات تاکیدی
        RuleFor(x => x.DailyAffirmation)
            .NotEmpty().WithMessage("جمله تاکیدی امروز نباید خالی باشد.");

        // ۴. اعتبارسنجی میزان مصرف آب (باید منطقی و مثبت باشد، مثلاً حداکثر ۲۰ لیوان)
        RuleFor(x => x.WaterIntake)
            .GreaterThanOrEqualTo(0).WithMessage("میزان مصرف آب نمی‌تواند منفی باشد.")
            .LessThanOrEqualTo(20).WithMessage("میزان مصرف آب وارد شده غیرمنطقی است.");

        // ۵. بررسی منطقی ساعت‌ها (ساعت خواب شب باید بعد از ساعت بیدارباش صبح باشد)
        // این یک ولیدیشن پیشرفته و کاملاً بیزینسی است!
        RuleFor(x => x)
            .Must(x => x.BedTime > x.WakeUpTime)
            .WithMessage("ساعت خواب شبانه باید بعد از ساعت بیدارباش صبحگاهی باشد.");
    }
}