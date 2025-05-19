using Cel.Core.Mediator.Enums;
using Cel.Core.Mediator.Models;
using Cel.GameOfLife.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cel.GameOfLife.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsFailure)
        {
            var errorResponse = new ApiResponse<T>() { Success = false, errors = result.Errors.Select(e => e.Description).ToList() };
            return result.Errors.Any(e => e.Type == ErrorType.NotFound) ? NotFound(errorResponse)
                : BadRequest(errorResponse);
        }

        return Ok(new ApiResponse<T>(result.Value));
    }

    protected ActionResult HandleResult(Result result)
    {
        if (result.IsFailure)
        {
            var errorResponse = new ApiResponse<bool>(false, false) { errors = result.Errors.Select(e => e.Description).ToList() };
            return result.Errors.Any(e => e.Type == ErrorType.NotFound) ? NotFound(errorResponse)
                : BadRequest(errorResponse);
        }

        return Ok(new ApiResponse<bool>(true));
    }
}
