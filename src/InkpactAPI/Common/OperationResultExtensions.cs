using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace InkpactAPI.Common;

public static class OperationResultExtensions
{
    public static IActionResult ToActionResult<T>(this OperationResult<T> result)
    {
        return result.Status switch
        {
            OperationResultStatus.Ok => new OkObjectResult(result.Data),
            OperationResultStatus.NotFound => new NotFoundObjectResult(new { error = result.Error }),
            OperationResultStatus.BadRequest => new BadRequestObjectResult(new { error = result.Error }),
            OperationResultStatus.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error }),
            OperationResultStatus.Conflict => new ConflictObjectResult(new { error = result.Error }),
            _ => new StatusCodeResult(500)
        };
    }
}
