using System.Net;
using System.Text.Json;
using Api.Responses;

namespace Api.Middlewares;


public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var error = new ErrorResponse(
                "Internal Server Error",
                HttpStatusCode.InternalServerError.ToString(),
                e.Message);
            string serialized = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(serialized);
        }
    }
}