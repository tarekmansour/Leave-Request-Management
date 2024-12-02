using Domain.Services;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class User
{
    public UserId Id { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;

    public User() { } // For EF Core

    public User(
        UserId id,
        string email,
        string firstName,
        string lastName)
    {
        if (!EmailValidator.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid user Email.", nameof(email));
        }

        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}
