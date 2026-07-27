using Mapster;
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
        var moodLog = await _moodLogRepository.GetByIdAsync(request.Id, cancellationToken);

        if (moodLog == null)
        {
            return null;
        }
        var response = moodLog.Adapt<MoodLogResponse>();

        return response;
    }
}