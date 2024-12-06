using Api.Contracts;
using Api.Extensions;
using Application.Commands.UserLogin;
using Application.Commands.UserRegister;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register new user.")]
    [SwaggerResponse(200, Type = typeof(int))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(
            Email: request.Email,
            FirstName: request.FirstName,
            LastName: request.LastName,
            Password: request.Password,
            Roles: request.Roles);

        var result = await _mediator.Send(command, cancellationToken);

        return result.ToApiResponse();
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login to generate user JWT token.")]
    [SwaggerResponse(200, Type = typeof(string))]
    [SwaggerResponse(400, "Validation errors occurred.")]
    [SwaggerResponse(404, "User not found.")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(
            Email: request.Email,
            Password: request.Password);

        var result = await _mediator.Send(command, cancellationToken);

        return result.ToApiResponse();
    }
}
