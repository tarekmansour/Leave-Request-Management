using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(result.Errors.Any() ? result.Errors : result.Error),
            ErrorType.NotFound => new NotFoundObjectResult(result.Error),
            _ => new ObjectResult(result.Error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
        };
    }
}