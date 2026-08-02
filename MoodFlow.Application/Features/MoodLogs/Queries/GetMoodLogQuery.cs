using Mapster;
using MediatR;
using MoodFlow.Application.Common.Interfaces;
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
    private readonly ICurrentUserService _currentUserService; // ۱. اضافه شدن سرویس کاربر جاری
    public GetMoodLogQueryHandler(IMoodLogRepository moodLogRepository, ICurrentUserService currentUserService)
    {
        _moodLogRepository = moodLogRepository;
        _currentUserService = currentUserService;
    }

    public async Task<MoodLogResponse?> Handle(GetMoodLogQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }
        //var moodLog = await _moodLogRepository.GetByIdAsync(request.Id, cancellationToken);
        var moodLog = await _moodLogRepository.GetByIdAndUserIdAsync(request.Id, userId, cancellationToken);
        if (moodLog == null)
        {
            return null;
        }
        var response = moodLog.Adapt<MoodLogResponse>();

        return response;
    }
}