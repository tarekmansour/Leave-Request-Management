using MediatR;
using SharedKernel;

namespace Application.Commands.Login;
public record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<string>>;
