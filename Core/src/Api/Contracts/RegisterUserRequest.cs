using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record RegisterUserRequest(
    [Required] string Email,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Password,
    [Required] IEnumerable<string> Roles);