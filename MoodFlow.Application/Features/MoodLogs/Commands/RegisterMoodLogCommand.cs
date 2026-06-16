using MediatR;
using Moodflow.Domain.Entities;
using MoodFlow.Domain.Enums;

namespace MoodFlow.Application.Features.MoodLogs.Commands;

// ۱. حالا تمام فیلدهای دفترچه را به عنوان ورودی از کاربر می‌گیریم
public record RegisterMoodLogCommand(
    int WakeUpTime,
    int EnergyLevel,
    MoodEmoji MorningMoodScore,
    string Gratitude1, string Gratitude2, string Gratitude3,
    string WhatWouldMakeTodayGreat1, string WhatWouldMakeTodayGreat2, string WhatWouldMakeTodayGreat3,
    string DailyAffirmation,
    string AmazingThingsHappened1, string AmazingThingsHappened2, string AmazingThingsHappened3,
    string HowCouldIHaveMadeTodayBetter,
    int WaterIntake,
    int BedTime
) : IRequest<long>; // خروجی ما long (همان Id) خواهد بود


// ۲. هندلری که اطلاعات بالا را می‌گیرد و یک نمونه از MoodLog می‌سازد
public class RegisterMoodLogCommandHandler : IRequestHandler<RegisterMoodLogCommand, long>
{
    public async Task<long> Handle(RegisterMoodLogCommand request, CancellationToken cancellationToken)
    {
        // تبدیل دیتای ورودی به شیء اصلی لایه دومین
        var moodLog = new MoodLog
        {
            Date = DateTime.UtcNow,
            WakeUpTime = request.WakeUpTime,
            EnergyLevel = request.EnergyLevel,
            MorningMoodScore = request.MorningMoodScore,
            Gratitude1 = request.Gratitude1,
            Gratitude2 = request.Gratitude2,
            Gratitude3 = request.Gratitude3,
            WhatWouldMakeTodayGreat1 = request.WhatWouldMakeTodayGreat1,
            WhatWouldMakeTodayGreat2 = request.WhatWouldMakeTodayGreat2,
            WhatWouldMakeTodayGreat3 = request.WhatWouldMakeTodayGreat3,
            DailyAffirmation = request.DailyAffirmation,
            AmazingThingsHappened1 = request.AmazingThingsHappened1,
            AmazingThingsHappened2 = request.AmazingThingsHappened2,
            AmazingThingsHappened3 = request.AmazingThingsHappened3,
            HowCouldIHaveMadeTodayBetter = request.HowCouldIHaveMadeTodayBetter,
            WaterIntake = request.WaterIntake,
            BedTime = request.BedTime
        };

        // فعلاً به صورت فیک یک آی‌دی تصادفی بلند برمی‌گردانیم تا بعداً به دیتابیس وصل شویم
        moodLog.Id = new Random().Next(1, 100000);

        return await Task.FromResult(moodLog.Id);
    }
}