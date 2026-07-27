using Mapster;
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

        // ۲. نگاشت کل لیست انتیتی‌ها به لیست MoodLogResponse فقط در یک خط!
        var responseList = items.Adapt<List<MoodLogResponse>>();

        return new PagedList<MoodLogResponse>(responseList, totalCount, request.PageNumber, request.PageSize);
    }
}
