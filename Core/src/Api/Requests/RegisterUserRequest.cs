using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record RegisterUserRequest(
    [Required] string Email,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Password);