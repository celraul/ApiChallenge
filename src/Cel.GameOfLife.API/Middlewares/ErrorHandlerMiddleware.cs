using Cel.GameOfLife.API.Models;
using Cel.GameOfLife.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Cel.GameOfLife.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var apiResponse = new ApiResponse<string>() { Success = false };
        int statusCode = (int)HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case ValidationException:
                apiResponse.errors = ((BaseException)exception).Errors;
                break;
            case NotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                apiResponse.errors = ((BaseException)exception).Errors;
                break;
            default:
                break;
        }

        response.StatusCode = statusCode;

        return response.WriteAsync(JsonSerializer.Serialize(apiResponse));
    }
}