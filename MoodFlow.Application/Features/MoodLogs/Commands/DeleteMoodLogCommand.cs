using MediatR;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Commands;

public record DeleteMoodLogCommand(long Id) : IRequest<Unit>;
public class DeleteMoodLogCommandHandler : IRequestHandler<DeleteMoodLogCommand, Unit>
{
    private readonly IMoodLogRepository _repository;
    private readonly ICurrentUserService _currentUserService; 
    public DeleteMoodLogCommandHandler(IMoodLogRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteMoodLogCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }
        // ۱. واکشی رکورد برای اطمینان از وجود آن
        var log = await _repository.GetByIdAndUserIdAsync(request.Id, userId, cancellationToken);

        if (log == null)
        {
            throw new KeyNotFoundException($"یادداشتی با شناسه {request.Id} برای حذف پیدا نشد.");
        }

        // ۲. نشانه گذاری برای حذف در حافظه (RAM)
        await _repository.DeleteAsync(log, cancellationToken);

        // ۳. ذخیره‌سازی قطعی و صدور دستور DELETE در دیتابیس
        await _repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}