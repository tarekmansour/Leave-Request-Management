using Api.Contracts;
using Application.Commands.CreateLeaveRequest;
using Application.Commands.UpdateLeaveRequest;
using Application.Dtos;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
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

    [Authorize(Roles = "User")]
    [HttpPost]
    [SwaggerOperation(Summary = "Submits a new leave request for a user.")]
    [SwaggerResponse(200, Type = typeof(int))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    public async Task<IActionResult> CreateLeaveRequestAsync(
            [FromBody] CreateLeaveRequest request,
            CancellationToken cancellationToken = default)
    {
        var command = new CreateLeaveRequestCommand(
            SubmittedBy: request.SubmittedBy,
            LeaveType: request.LeaveType.ToString(),
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Comment: request.Comment);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(result.Value);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPatch("{id}")]
    [SwaggerResponse(200, Type = typeof(UpdatedLeaveRequestDto))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    [SwaggerResponse(401, "Unauthorized user.")]
    [SwaggerResponse(403, "Forbidden access.")]
    [SwaggerOperation(Summary = "Update existing leave request.")]
    public async Task<IActionResult> UpdateLeaveRequestAsync(
        int id,
        [FromBody] UpdateLeaveRequest updateLeaveRequest,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateLeaveRequestCommand(
            LeaveRequestId: id,
            LeaveType: updateLeaveRequest.LeaveType?.ToString(),
            StartDate: updateLeaveRequest.StartDate,
            EndDate: updateLeaveRequest.EndDate,
            NewStatus: updateLeaveRequest.NewStatus?.ToString(),
            Comment: updateLeaveRequest.Comment,
            DecisionReason: updateLeaveRequest.DecisionReason);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsFailure
            ? BadRequest(result.Errors is IEnumerable<Error> errorList && errorList.Any()
                ? errorList
                : result.Error)
            : Ok(result.Value);
    }
}