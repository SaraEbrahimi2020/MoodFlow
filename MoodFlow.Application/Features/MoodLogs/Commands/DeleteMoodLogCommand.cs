using MediatR;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Commands;

public record DeleteMoodLogCommand(long Id) : IRequest<Unit>;
public class DeleteMoodLogCommandHandler : IRequestHandler<DeleteMoodLogCommand, Unit>
{
    private readonly IMoodLogRepository _repository;

    public DeleteMoodLogCommandHandler(IMoodLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteMoodLogCommand request, CancellationToken cancellationToken)
    {
        // ۱. واکشی رکورد برای اطمینان از وجود آن
        var log = await _repository.GetByIdAsync(request.Id, cancellationToken);

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