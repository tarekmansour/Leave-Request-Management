using MediatR;
using SharedKernel;

namespace Application.Commands.UserLogin;
public record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<string>>;
