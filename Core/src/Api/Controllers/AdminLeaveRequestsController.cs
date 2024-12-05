using Api.Contracts;
using Application.Commands.UpdateLeaveRequest;
using Application.Dtos;
using Asp.Versioning;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Authorize(Roles = Roles.HR)]
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
[Produces("application/json")]
public class AdminLeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminLeaveRequestsController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Update existing leave request.")]
    [SwaggerResponse(200, Type = typeof(UpdatedLeaveRequestDto))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    [SwaggerResponse(401, "Unauthorized user.")]
    [SwaggerResponse(403, "Forbidden access.")]
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
            Status: updateLeaveRequest.Status?.ToString(),
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
