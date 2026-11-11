using MediatR;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Queries.GetMoodSummary;

public record MoodSummaryResponse(
    int TotalLogsCount,              // تعداد کل روزهای ثبت‌شده
    double AverageEnergyLevel,       // میانگین سطح انرژی
    double AverageWaterIntake,       // میانگین مصرف آب
    string? MostFrequentMorningMood, // شایع‌ترین ایموجی/حس صبحگاهی
    int CurrentStreak                // اضافه شدن زنجیره روزهای متوالی
);
public record GetMoodSummaryQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<MoodSummaryResponse>;
public class GetMoodSummaryQueryHandler : IRequestHandler<GetMoodSummaryQuery, MoodSummaryResponse>
{
    private readonly IMoodLogRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetMoodSummaryQueryHandler(IMoodLogRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<MoodSummaryResponse> Handle(GetMoodSummaryQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }

        var (items, totalCount) = await _repository.GetPagedByUserIdAsync(
            userId,
            pageNumber: 1,
            pageSize: 1000,
            fromDate: request.FromDate,
            toDate: request.ToDate,
            cancellationToken);

        // اگر هیچ لاگی وجود نداشت (رفع خطای تعداد پارامتر با دادن 0 برای Streak)
        if (!items.Any())
        {
            return new MoodSummaryResponse(0, 0, 0, null, 0);
        }

        // انجام محاسبات آماری با LINQ
        var totalLogs = items.Count;
        var avgEnergy = items.Average(x => x.EnergyLevel);
        var avgWater = items.Average(x => x.WaterIntake);

        // پیدا کردن بیشترین ایموجی ثبت‌شده در صبح
        var mostFrequentMood = items
            .GroupBy(x => x.MorningMoodScore)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key.ToString())
            .FirstOrDefault();

        // -------------------------------------------------------------
        // ۲. الگوریتم جدید: محاسبه زنجیره روزهای متوالی (Current Streak)
        // -------------------------------------------------------------
        var orderedLogs = items.OrderByDescending(x => x.Date.Date).ToList();

        int currentStreak = 0;
        var expectedDate = DateTime.UtcNow.Date;

        foreach (var log in orderedLogs)
        {
            var logDate = log.Date.Date;

            // اگر کاربر لاگ امروز یا دیروز را ثبت کرده باشد زنجیره فعال است
            if (logDate == expectedDate || (currentStreak == 0 && logDate == expectedDate.AddDays(-1)))
            {
                currentStreak++;
                expectedDate = logDate.AddDays(-1);
            }
            else if (logDate < expectedDate)
            {
                // زنجیره قطع شده است
                break;
            }
        }

        // ۳. بازگرداندن پاسخ کامل به همراه CurrentStreak
        return new MoodSummaryResponse(
            TotalLogsCount: totalLogs,
            AverageEnergyLevel: Math.Round(avgEnergy, 1),
            AverageWaterIntake: Math.Round(avgWater, 1),
            MostFrequentMorningMood: mostFrequentMood,
            CurrentStreak: currentStreak
        );
    }
}