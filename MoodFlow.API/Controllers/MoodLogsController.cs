using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodFlow.Application.Features.MoodLogs.Commands;
using MoodFlow.Application.Features.MoodLogs.Queries;
using MoodFlow.Application.Features.MoodLogs.Queries.GetMoodSummary;

namespace MoodFlow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MoodLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    // تزریق MediatR برای فرستادن کمندها و کوئری‌ها
    public MoodLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    //[HttpGet]
    //public async Task<ActionResult<List<MoodLogResponse>>> GetAll(CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(new GetMoodLogsQuery(), cancellationToken);
    //    return Ok(result);
    //}
    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] GetMoodLogsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<MoodLogResponse>> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetMoodLogQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound("دفترچه‌ای با این شناسه پیدا نشد.");
        }

        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateMoodLogCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateMoodLogCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(new { Message = "شناسه مسیر درخواست با شناسه دیتای ارسالی همخوانی ندارد." });
        }

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMoodLogCommand(id), cancellationToken);

        // پاسخ ۲۰۴ طبق استاندارد REST
        return NoContent();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<MoodSummaryResponse>> GetSummary(
    [FromQuery] DateTime? fromDate,
    [FromQuery] DateTime? toDate,
    CancellationToken cancellationToken)
    {
        var query = new GetMoodSummaryQuery(fromDate, toDate);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

}