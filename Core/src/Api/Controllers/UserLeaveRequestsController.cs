using Api.Contracts;
using Api.Extensions;
using Application.Commands.CreateLeaveRequest;
using Application.Dtos;
using Application.Queries.GetLeaveRequestsByUser;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/me/[controller]")]
[Produces("application/json")]
public class UserLeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserLeaveRequestsController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    [SwaggerOperation(Summary = "Submits my new leave request.")]
    [SwaggerResponse(200, Type = typeof(int))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    [SwaggerResponse(401, "Unauthorized user.")]
    public async Task<IActionResult> CreateLeaveRequestAsync(
            [FromBody] CreateLeaveRequest request,
            CancellationToken cancellationToken = default)
    {
        var command = new CreateLeaveRequestCommand(
            LeaveType: request.LeaveType.ToString(),
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Comment: request.Comment);

        var result = await _mediator.Send(command, cancellationToken);

        return result.ToApiResponse();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get my list of leave requests.")]
    [SwaggerResponse(200, Type = typeof(LeaveRequestsCollectionDto))]
    [SwaggerResponse(401, "Unauthorized user.")]
    public async Task<IActionResult> GetUserLeaveRequests(
        [FromQuery] string? status,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLeaveRequestsByUserQuery(status);

        var result = await _mediator.Send(query, cancellationToken);

        return result.ToApiResponse();
    }
}