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
            return BadRequest(new ApiResponse<string>() { errors = result.Errors.Select(e => e.Description).ToList() });

        return Ok(new ApiResponse<T>(result.Value));
    }
}
