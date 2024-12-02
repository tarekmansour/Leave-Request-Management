using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);
