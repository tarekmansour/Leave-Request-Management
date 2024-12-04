using MediatR;
using SharedKernel;

namespace Application.Commands.UserRegister;
public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    IEnumerable<string> Roles) : IRequest<Result<int>>;
