using MediatR;
using MoodFlow.Domain.Enums;

namespace MoodFlow.Application.Features.MoodLogs.Queries;

public class MoodLogResponse
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public int WakeUpTime { get; set; }
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
    public int BedTime { get; set; }
}
public record GetMoodLogQuery(long Id) : IRequest<MoodLogResponse>;

public class GetMoodLogQueryHandler : IRequestHandler<GetMoodLogQuery, MoodLogResponse>
{
    public async Task<MoodLogResponse> Handle(GetMoodLogQuery request, CancellationToken cancellationToken)
    {
        // فعلاً به صورت فیک یک پاسخ پر از دیتای دفترچه می‌سازیم تا ببینیم در سواگر چطور دیده می‌شود
        var response = new MoodLogResponse
        {
            Id = request.Id,
            Date = DateTime.UtcNow,
            WakeUpTime = 7,
            EnergyLevel = 4,
            MorningMoodScore = MoodEmoji.Excellent,// عالی 😊
            Gratitude1 = "بابت پروژه شخصی جدیدم",
            Gratitude2 = "بابت یادگیری CQRS",
            Gratitude3 = "بابت حس خوبی که امروز دارم",
            WhatWouldMakeTodayGreat1 = "کدها بدون خطا Build بشن",
            DailyAffirmation = "من هر روز در حال رشد و یادگیری هستم",
            AmazingThingsHappened1 = "اولین خروجی زنده رو در سواگر دیدم!",
            WaterIntake = 6,
            BedTime = 23
        };

        return await Task.FromResult(response);
    }
}