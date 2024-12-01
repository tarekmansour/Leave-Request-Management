using Domain.Services;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class Employee
{
    public EmployeeId Id { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public EmployeePosition Position { get; private set; } = default!;

    public Employee() { } // For EF Core

    public Employee(string firstName, string lastName, string email, EmployeePosition position)
    {
        if (!EmailValidator.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid Employee Email.", nameof(email));
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Position = position;
    }

    public Employee(EmployeeId id, string firstName, string lastName, string email, EmployeePosition position)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Position = position;
    }
}
