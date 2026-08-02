using MediatR;
using Moodflow.Domain.Entities;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Domain.Enums;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Commands;

// ۱. حالا تمام فیلدهای دفترچه را به عنوان ورودی از کاربر می‌گیریم
public record CreateMoodLogCommand(
    TimeOnly WakeUpTime,
    int EnergyLevel,
    MoodEmoji MorningMoodScore,
    string Gratitude1, string Gratitude2, string Gratitude3,
    string WhatWouldMakeTodayGreat1, string WhatWouldMakeTodayGreat2, string WhatWouldMakeTodayGreat3,
    string DailyAffirmation,
    string AmazingThingsHappened1, string AmazingThingsHappened2, string AmazingThingsHappened3,
    string HowCouldIHaveMadeTodayBetter,
    int WaterIntake,
    TimeOnly BedTime
) : IRequest<long>; // خروجی ما long (همان Id) خواهد بود


// ۲. هندلری که اطلاعات بالا را می‌گیرد و یک نمونه از MoodLog می‌سازد
public class CreateMoodLogCommandHandler : IRequestHandler<CreateMoodLogCommand, long>
{
    private readonly IMoodLogRepository _moodLogRepository;
    private readonly ICurrentUserService _currentUserService;
    public CreateMoodLogCommandHandler(IMoodLogRepository moodLogRepository, ICurrentUserService currentUserService)
    {
        _moodLogRepository = moodLogRepository;
        _currentUserService = currentUserService;
    }

    public async Task<long> Handle(CreateMoodLogCommand request, CancellationToken cancellationToken)
    {
        // گرفتن شناسه کاربر جاری از توکن
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }
        // ۱. ساخت یک انتیتی جدید بر اساس اطلاعاتی که کاربر فرستاده (Request)
        var moodLog = MoodLog.Create(

            request.WakeUpTime,
            request.EnergyLevel,
            request.MorningMoodScore,
            request.Gratitude1,
            request.Gratitude2,
            request.Gratitude3,
            request.WhatWouldMakeTodayGreat1,
            request.WhatWouldMakeTodayGreat2,
            request.WhatWouldMakeTodayGreat3,
            request.DailyAffirmation,
            request.AmazingThingsHappened1,
            request.AmazingThingsHappened2,
            request.AmazingThingsHappened3,
            request.HowCouldIHaveMadeTodayBetter,
            request.WaterIntake,
            userId,
            request.BedTime

        );

        // ۲. اضافه کردن انتیتی به دی‌بی‌کانتکست از طریق ریپازیتوری
        await _moodLogRepository.AddAsync(moodLog, cancellationToken);

        // ۳. ذخیره نهایی و قطعی تغییرات در SQL Server
        await _moodLogRepository.SaveChangesAsync(cancellationToken);

        // ۴. بازگرداندن آی‌دی رکورد جدید ساخته شده
        return moodLog.Id;
    }
}