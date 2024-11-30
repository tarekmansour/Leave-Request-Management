using Domain.Services;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class Employee
{
    public EmployeeId Id { get; private set; } = default!;
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public EmployeePosition Position { get; private set; }

    public Employee(string firstName, string lastName, string email, EmployeePosition position)
    {
        if (!EmailValidator.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid employee Email.", nameof(email));
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Position = position;
    }
}
