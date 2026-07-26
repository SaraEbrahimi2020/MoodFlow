using MediatR;
using MoodFlow.Application.Common.Models;
using MoodFlow.Domain.Repositories;


namespace MoodFlow.Application.Features.MoodLogs.Queries;

public record GetMoodLogsWithPaginationQuery : IRequest<PagedList<MoodLogResponse>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    // پارامترهای اختیاری برای فیلتر تاریخ
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}
public class GetMoodLogsWithPaginationQueryHandler
    : IRequestHandler<GetMoodLogsWithPaginationQuery, PagedList<MoodLogResponse>>
{
    private readonly IMoodLogRepository _repository;

    public GetMoodLogsWithPaginationQueryHandler(IMoodLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedList<MoodLogResponse>> Handle(
        GetMoodLogsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.FromDate,
            request.ToDate,
            cancellationToken);

        // نگاشت انتیتی به MoodLogResponse
        var responseList = items.Select(x => new MoodLogResponse
        {
            Id = x.Id,
            Date = x.Date,
            WakeUpTime = x.WakeUpTime,
            EnergyLevel = x.EnergyLevel,
            MorningMoodScore = x.MorningMoodScore,
            Gratitude1 = x.Gratitude1,
            Gratitude2 = x.Gratitude2,
            Gratitude3 = x.Gratitude3,
            WhatWouldMakeTodayGreat1 = x.WhatWouldMakeTodayGreat1,
            WhatWouldMakeTodayGreat2 = x.WhatWouldMakeTodayGreat2,
            WhatWouldMakeTodayGreat3 = x.WhatWouldMakeTodayGreat3,
            DailyAffirmation = x.DailyAffirmation,
            AmazingThingsHappened1 = x.AmazingThingsHappened1,
            AmazingThingsHappened2 = x.AmazingThingsHappened2,
            AmazingThingsHappened3 = x.AmazingThingsHappened3,
            HowCouldIHaveMadeTodayBetter = x.HowCouldIHaveMadeTodayBetter,
            WaterIntake = x.WaterIntake,
            BedTime = x.BedTime
        }).ToList();

        return new PagedList<MoodLogResponse>(responseList, totalCount, request.PageNumber, request.PageSize);
    }
}
