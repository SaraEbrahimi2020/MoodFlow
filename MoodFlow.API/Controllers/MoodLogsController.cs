using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoodFlow.Application.Features.MoodLogs.Queries; // آدرس کوئری که خودت نوشتی

namespace MoodFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoodLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    // تزریق وابستگی (DI) برای مدیاآر
    public MoodLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // این متد ای‌پ‌ی‌آی برای گرفتن یک مود با آی‌دی است
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        // ۱. پاکت نامه (کوئری تو) را می‌سازد
        var query = new GetMoodLogQuery(id);

        // ۲. آن را به مدیاآر شلیک می‌کند
        var response = await _mediator.Send(query);

        // ۳. خروجی را با وضعیت ۲۰۰ (OK) برمی‌گرداند
        return Ok(response);
    }
}