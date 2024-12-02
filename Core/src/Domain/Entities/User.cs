using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class User
{
    public USerId Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public User(
        USerId id,
        string email,
        string firstName,
        string lastName)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}
