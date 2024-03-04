using System.Net;
using Domain.Errors;

namespace Api.Responses;

public record ErrorResponse(string Title, string Status, string Details);

internal static class ErrorResponseExtensions
{
    public static ErrorResponse MapToErrorResponse(this ApplicationError error)
    {
        switch (error)
        {
            case var _ when error is NotFoundError:
                return new ErrorResponse(error.Title, HttpStatusCode.NotFound.ToString(), error.Details);
            case var _ when error is AlreadyExistsError:
                return new ErrorResponse(error.Title, HttpStatusCode.Conflict.ToString(), error.Details);
            default:
                return new ErrorResponse(error.Title, HttpStatusCode.BadRequest.ToString(), error.Details);
        }
    }
}