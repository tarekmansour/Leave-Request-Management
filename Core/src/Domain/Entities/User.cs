using Domain.Services;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class User
{
    public UserId Id { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public IEnumerable<string> Roles { get; private set; } = default!;


    public User() { } // For EF Core

    public User(
        string email,
        string firstName,
        string lastName,
        string passwordHash,
        IEnumerable<string> roles)
    {
        if (!EmailValidator.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid user Email.", nameof(email));
        }

        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Roles = roles;
    }

    public User(
        UserId id,
        string email,
        string firstName,
        string lastName,
        string passwordHash)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
    }
}
