using MoodFlow.Domain.Enums;

namespace Moodflow.Domain.Entities;

public class MoodLog
{
    // ۱. اطلاعات اصلی روز
    public long Id { get;private set; }
    public DateTime Date { get; private set; } = DateTime.UtcNow.Date;

    // ۲. اطلاعات بخش صبح (خورشید)
    public TimeOnly WakeUpTime { get;private set; }
    public int EnergyLevel { get;private set; }
    public MoodEmoji MorningMoodScore { get;private set; }
    public string Gratitude1 { get;private set; } = string.Empty;
    public string Gratitude2 { get;private set; } = string.Empty;
    public string Gratitude3 { get;private set; } = string.Empty;

    public string WhatWouldMakeTodayGreat1 { get;private set; } = string.Empty;
    public string WhatWouldMakeTodayGreat2 { get;private set; } = string.Empty;
    public string WhatWouldMakeTodayGreat3 { get;private set; } = string.Empty;

    public string DailyAffirmation { get;private set; } = string.Empty;

    // ۳. اطلاعات بخش شب (ماه)
    public string AmazingThingsHappened1 { get;private set; } = string.Empty;
    public string AmazingThingsHappened2 { get;private set; } = string.Empty;
    public string AmazingThingsHappened3 { get;private set; } = string.Empty;

    public string HowCouldIHaveMadeTodayBetter { get; private set; } = string.Empty;

    public int WaterIntake { get; private set; }
    public TimeOnly BedTime { get;private  set; }
    private MoodLog() { }

    // ۳. متد استاتیک اختصاصی برای ساخت یک لاگ جدید و اصولی
    public static MoodLog Create(
        TimeOnly wakeUpTime,
        int energyLevel,
        MoodEmoji morningMoodScore,
        string gratitude1,
        string gratitude2,
        string gratitude3,
        string whatWouldMakeTodayGreat1,
        string whatWouldMakeTodayGreat2,
        string whatWouldMakeTodayGreat3,
        string dailyAffirmation,
        string amazingThingsHappened1,
        string amazingThingsHappened2,
        string amazingThingsHappened3,
        string howCouldIHaveMadeTodayBetter,
        int waterIntake,
        TimeOnly bedTime)
    {
        // اینجا حتی می‌توانیم ولیدیشن‌های دامین (قوانین همیشگی بیزینس) را هم چک کنیم!

        return new MoodLog
        {
            WakeUpTime = wakeUpTime,
            EnergyLevel = energyLevel,
            MorningMoodScore = morningMoodScore,
            Gratitude1 = gratitude1,
            Gratitude2 = gratitude2,
            Gratitude3 = gratitude3,
            WhatWouldMakeTodayGreat1 = whatWouldMakeTodayGreat1,
            WhatWouldMakeTodayGreat2 = whatWouldMakeTodayGreat2,
            WhatWouldMakeTodayGreat3 = whatWouldMakeTodayGreat3,
            DailyAffirmation = dailyAffirmation,
            AmazingThingsHappened1 = amazingThingsHappened1,
            AmazingThingsHappened2 = amazingThingsHappened2,
            AmazingThingsHappened3 = amazingThingsHappened3,
            HowCouldIHaveMadeTodayBetter = howCouldIHaveMadeTodayBetter,
            WaterIntake = waterIntake,
            BedTime = bedTime,
            Date = DateTime.UtcNow 
        };
    }
    public void Update(
    TimeOnly wakeUpTime,
    int energyLevel,
    MoodEmoji morningMoodScore,
    string gratitude1,
    string gratitude2,
    string gratitude3,
    string whatWouldMakeTodayGreat1,
    string whatWouldMakeTodayGreat2,
    string whatWouldMakeTodayGreat3,
    string dailyAffirmation,
    string amazingThingsHappened1,
    string amazingThingsHappened2,
    string amazingThingsHappened3,
    string howCouldIHaveMadeTodayBetter,
    int waterIntake,
    TimeOnly bedTime)
    {
        WakeUpTime = wakeUpTime;
        EnergyLevel = energyLevel;
        MorningMoodScore = morningMoodScore;
        Gratitude1 = gratitude1;
        Gratitude2 = gratitude2;
        Gratitude3 = gratitude3;
        WhatWouldMakeTodayGreat1 = whatWouldMakeTodayGreat1;
        WhatWouldMakeTodayGreat2 = whatWouldMakeTodayGreat2;
        WhatWouldMakeTodayGreat3 = whatWouldMakeTodayGreat3;
        DailyAffirmation = dailyAffirmation;
        AmazingThingsHappened1 = amazingThingsHappened1;
        AmazingThingsHappened2 = amazingThingsHappened2;
        AmazingThingsHappened3 = amazingThingsHappened3;
        HowCouldIHaveMadeTodayBetter = howCouldIHaveMadeTodayBetter;
        WaterIntake = waterIntake;
        BedTime = bedTime;

        // اگر فیلدی مثل ModifiedAt داری، اینجا می‌توانی زمانش را هم به روز کنی:
        // ModifiedAt = DateTime.UtcNow;
    }

}