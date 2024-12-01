using Api.Requests;
using Application.Commands.CreateLeaveRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestsController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    public async Task<IActionResult> CreateLeaveRequestAsync(
            [FromBody] CreateLeaveRequest request,
            CancellationToken cancellationToken)
    {
        var command = new CreateLeaveRequestCommand(
            EmployeeId: request.EmployeeId,
            LeaveTypeId: request.LeaveTypeId,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Comment: request.Comment);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(result.Value);
    }
}
