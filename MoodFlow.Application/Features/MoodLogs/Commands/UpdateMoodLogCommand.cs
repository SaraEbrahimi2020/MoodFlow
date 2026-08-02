using MediatR;
using MoodFlow.Application.Common.Interfaces;
using MoodFlow.Domain.Enums;
using MoodFlow.Domain.Repositories;

namespace MoodFlow.Application.Features.MoodLogs.Commands;

// در ورودی شناسه رکوردی که باید تغییر کند را هم می‌گیریم
public record UpdateMoodLogCommand(
    long Id,
    TimeOnly WakeUpTime,
    int EnergyLevel,
    MoodEmoji MorningMoodScore,
    string Gratitude1,
    string Gratitude2,
    string Gratitude3,
    string WhatWouldMakeTodayGreat1,
    string WhatWouldMakeTodayGreat2,
    string WhatWouldMakeTodayGreat3,
    string DailyAffirmation,
    string AmazingThingsHappened1,
    string AmazingThingsHappened2,
    string AmazingThingsHappened3,
    string HowCouldIHaveMadeTodayBetter,
    int WaterIntake,
    TimeOnly BedTime
) : IRequest<Unit>; // Unit یعنی این کمند خروجی خاصی ندارد و فقط عملیات انجام می‌شود
public class UpdateMoodLogCommandHandler : IRequestHandler<UpdateMoodLogCommand, Unit>
{
    private readonly IMoodLogRepository _moodLogRepository;
    private readonly ICurrentUserService _currentUserService; 
    public UpdateMoodLogCommandHandler(IMoodLogRepository repository, ICurrentUserService currentUserService)
    {
        _moodLogRepository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateMoodLogCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");
        }
        // ۱. پیدا کردن رکورد قدیمی از دیتابیس
        var log = await _moodLogRepository.GetByIdAndUserIdAsync(request.Id, userId, cancellationToken);

        if (log == null)
        {
            throw new KeyNotFoundException($"یادداشتی با شناسه {request.Id} برای ویرایش پیدا نشد.");
        }

        // ۲. به‌روزرسانی فیلدهای انتیتی با دیتای جدید ورود
        log.Update(
            request.WakeUpTime,
            request.EnergyLevel,
            request.MorningMoodScore,
            request.Gratitude1,
            request.Gratitude2,
            request.Gratitude3,
            request.WhatWouldMakeTodayGreat1,
            request.WhatWouldMakeTodayGreat2,
            request.WhatWouldMakeTodayGreat3,
            request.DailyAffirmation,
            request.AmazingThingsHappened1,
            request.AmazingThingsHappened2,
            request.AmazingThingsHappened3,
            request.HowCouldIHaveMadeTodayBetter,
            request.WaterIntake,
            request.BedTime
        );

        // ۳. ذخیره تغییرات در دیتابیس
        await _moodLogRepository.UpdateAsync(log, cancellationToken);
        await _moodLogRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}