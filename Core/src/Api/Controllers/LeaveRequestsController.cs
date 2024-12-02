using Api.Requests;
using Application.Commands.CreateLeaveRequest;
using Asp.Versioning;
using Domain.ValueObjects.Identifiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
[Produces("application/json")]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestsController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    [SwaggerOperation(Summary = "Submits a new leave request for a user.")]
    [SwaggerResponse(200, Type = typeof(int))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    public async Task<IActionResult> CreateLeaveRequestAsync(
            [FromBody] CreateLeaveRequest request,
            CancellationToken cancellationToken)
    {
        var command = new CreateLeaveRequestCommand(
            SubmittedBy: new UserId(request.SubmittedBy),
            LeaveTypeId: new LeaveTypeId(request.LeaveTypeId),
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Comment: request.Comment);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(result.Value);
    }
}
