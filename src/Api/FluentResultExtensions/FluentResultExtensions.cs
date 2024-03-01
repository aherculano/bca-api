using System.Net;
using Api.Responses;
using Application.Errors;
using Domain.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.FluentResultExtensions;

internal static class FluentResultExtensions
{
    public static IActionResult ToFailedActionResult(this Result result)
    {
        return HandleErrorResult(result.ToResult());
    }

    public static IActionResult ToFailedActionResult<T>(this Result<T> result)
    {
        return HandleErrorResult(result);
    }

    private static ActionResult HandleErrorResult<T>(Result<T> result)
    {
        var errors = result.Errors;
        switch (errors)
        {
            case var _ when errors.Any(x => x is ValidationError):
                return new BadRequestObjectResult(new ErrorResponse("Validation Error",
                    HttpStatusCode.BadRequest.ToString(),
                    string.Join("; ",
                        (errors.FirstOrDefault() as ValidationError).Failures.Select(x => x.ErrorMessage))));
            case var _ when errors.Any(x => x is NotFoundError):
                return new NotFoundObjectResult((errors.FirstOrDefault() as ApplicationError)?.MapToErrorResponse());
            case var _ when errors.Any(x => x is AlreadyExistsError):
                return new ConflictObjectResult((errors.FirstOrDefault() as ApplicationError)?.MapToErrorResponse());
            case var _ when errors.Any(x => x is ConflictAuctionError):
                return new ConflictObjectResult((errors.FirstOrDefault() as ApplicationError)?.MapToErrorResponse());
            default:
                return new BadRequestObjectResult(new ErrorResponse("Bad Request", HttpStatusCode.BadRequest.ToString(),
                    "Bad Request"));
        }
    }
}