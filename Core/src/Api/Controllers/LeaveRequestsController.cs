using Api.Requests;
using Application.Commands.CreateLeaveRequest;
using Application.Dtos;
using Domain.ValueObjects.Identifiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestsController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    [SwaggerOperation(Summary = "Submits a new leave request for an employee.")]
    [SwaggerResponse(200, Type = typeof(CreatedLeaveRequestDto))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    public async Task<IActionResult> CreateLeaveRequestAsync(
            [FromBody] CreateLeaveRequest request,
            CancellationToken cancellationToken)
    {
        var command = new CreateLeaveRequestCommand(
            EmployeeId: new EmployeeId(request.EmployeeId),
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
