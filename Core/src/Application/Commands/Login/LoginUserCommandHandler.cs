using MediatR;
using SharedKernel;

namespace Application.Commands.Login;
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    public Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
