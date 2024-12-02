using Domain.Entities;

namespace Domain.Abstractions;
public interface ITokenProvider
{
    string GenerateToken(User user);
}