using Domain.Entities;

namespace Application.Abstractions;
public interface ITokenProvider
{
    string GenerateToken(User user);
}
