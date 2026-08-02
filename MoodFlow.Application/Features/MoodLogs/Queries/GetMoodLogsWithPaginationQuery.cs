using Mapster;
using MediatR;
using MoodFlow.Application.Common.Interfaces;
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
    private readonly ICurrentUserService _currentUserService; // ۱. اضافه شدن سرویس
    public GetMoodLogsWithPaginationQueryHandler(IMoodLogRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedList<MoodLogResponse>> Handle(
        GetMoodLogsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }
        var (items, totalCount) = await _repository.GetPagedByUserIdAsync(
            userId,
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
