using MediatR;
using MoodFlow.Domain.Enums;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Queries;

public class MoodLogResponse
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly WakeUpTime { get; set; }
    public int EnergyLevel { get; set; }
    public MoodEmoji MorningMoodScore { get; set; }
    public string Gratitude1 { get; set; } = string.Empty;
    public string Gratitude2 { get; set; } = string.Empty;
    public string Gratitude3 { get; set; } = string.Empty;
    public string WhatWouldMakeTodayGreat1 { get; set; } = string.Empty;
    public string WhatWouldMakeTodayGreat2 { get; set; } = string.Empty;
    public string WhatWouldMakeTodayGreat3 { get; set; } = string.Empty;
    public string DailyAffirmation { get; set; } = string.Empty;
    public string AmazingThingsHappened1 { get; set; } = string.Empty;
    public string AmazingThingsHappened2 { get; set; } = string.Empty;
    public string AmazingThingsHappened3 { get; set; } = string.Empty;
    public string HowCouldIHaveMadeTodayBetter { get; set; } = string.Empty;
    public int WaterIntake { get; set; }
    public TimeOnly BedTime { get; set; }
}
public record GetMoodLogQuery(long Id) : IRequest<MoodLogResponse>;

public class GetMoodLogQueryHandler : IRequestHandler<GetMoodLogQuery, MoodLogResponse?>
{
    private readonly IMoodLogRepository _moodLogRepository;

    public GetMoodLogQueryHandler(IMoodLogRepository moodLogRepository)
    {
        _moodLogRepository = moodLogRepository;
    }

    public async Task<MoodLogResponse?> Handle(GetMoodLogQuery request, CancellationToken cancellationToken)
    {
        // خواندن دیتای واقعی با استفاده از Id که در رکورد تعریف کردی
        var moodLog = await _moodLogRepository.GetByIdAsync(request.Id, cancellationToken);

        if (moodLog == null)
        {
            return null;
        }

        // مپ کردن انتیتی دیتابیس به کلاس MoodLogResponse شما
        return new MoodLogResponse
        {
            Id = moodLog.Id,
            Date = moodLog.Date,
            WakeUpTime = moodLog.WakeUpTime,
            EnergyLevel = moodLog.EnergyLevel,
            MorningMoodScore = moodLog.MorningMoodScore,
            Gratitude1 = moodLog.Gratitude1,
            Gratitude2 = moodLog.Gratitude2,
            Gratitude3 = moodLog.Gratitude3,
            WhatWouldMakeTodayGreat1 = moodLog.WhatWouldMakeTodayGreat1,
            WhatWouldMakeTodayGreat2 = moodLog.WhatWouldMakeTodayGreat2,
            WhatWouldMakeTodayGreat3 = moodLog.WhatWouldMakeTodayGreat3,
            DailyAffirmation = moodLog.DailyAffirmation,
            AmazingThingsHappened1 = moodLog.AmazingThingsHappened1,
            AmazingThingsHappened2 = moodLog.AmazingThingsHappened2,
            AmazingThingsHappened3 = moodLog.AmazingThingsHappened3,
            HowCouldIHaveMadeTodayBetter = moodLog.HowCouldIHaveMadeTodayBetter,
            WaterIntake = moodLog.WaterIntake,
            BedTime = moodLog.BedTime
        };
    }
}