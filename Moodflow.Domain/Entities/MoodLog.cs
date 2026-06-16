using MoodFlow.Domain.Enums;

namespace Moodflow.Domain.Entities;

public class MoodLog
{
    // ۱. اطلاعات اصلی روز
    public long Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    // ۲. اطلاعات بخش صبح (خورشید)
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

    // ۳. اطلاعات بخش شب (ماه)
    public string AmazingThingsHappened1 { get; set; } = string.Empty;
    public string AmazingThingsHappened2 { get; set; } = string.Empty;
    public string AmazingThingsHappened3 { get; set; } = string.Empty;

    public string HowCouldIHaveMadeTodayBetter { get; set; } = string.Empty;

    public int WaterIntake { get; set; }
    public int BedTime { get; set; }
}