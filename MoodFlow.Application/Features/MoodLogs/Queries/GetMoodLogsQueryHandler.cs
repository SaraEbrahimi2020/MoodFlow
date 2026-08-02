using MediatR;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Queries;

public record GetMoodLogsQuery : IRequest<List<MoodLogResponse>>;
public class GetMoodLogsQueryHandler : IRequestHandler<GetMoodLogsQuery, List<MoodLogResponse>>
{
    private readonly IMoodLogRepository _repository;
    private readonly ICurrentUserService _currentUserService; // ۱. اضافه شدن سرویس کاربر جاری
    public GetMoodLogsQueryHandler(IMoodLogRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<MoodLogResponse>> Handle(GetMoodLogsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }
        // ۱. گرفتن تمام رکوردها از ریپازیتوری
        var logs = await _repository.GetByUserIdAsync(userId, cancellationToken);

        // ۲. تبدیل تک‌تک انتیتی‌ها به مدل پاسخ کامل شما به کمک Select
        return logs.Select(log => new MoodLogResponse
        {
            Id = log.Id,
            Date = log.Date,
            WakeUpTime = log.WakeUpTime,
            EnergyLevel = log.EnergyLevel,
            MorningMoodScore = log.MorningMoodScore,
            Gratitude1 = log.Gratitude1,
            Gratitude2 = log.Gratitude2,
            Gratitude3 = log.Gratitude3,
            WhatWouldMakeTodayGreat1 = log.WhatWouldMakeTodayGreat1,
            WhatWouldMakeTodayGreat2 = log.WhatWouldMakeTodayGreat2,
            WhatWouldMakeTodayGreat3 = log.WhatWouldMakeTodayGreat3,
            DailyAffirmation = log.DailyAffirmation,
            AmazingThingsHappened1 = log.AmazingThingsHappened1,
            AmazingThingsHappened2 = log.AmazingThingsHappened2,
            AmazingThingsHappened3 = log.AmazingThingsHappened3,
            HowCouldIHaveMadeTodayBetter = log.HowCouldIHaveMadeTodayBetter,
            WaterIntake = log.WaterIntake,
            BedTime = log.BedTime
        }).ToList();
    }
}