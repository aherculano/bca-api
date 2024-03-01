using FluentResults;

namespace Domain.FluentResults;

public static class FluentResultExtensions
{
    public static Result<T> ThrowExceptionIfHasFailedResult<T>(this Result<T> result)
    {
        if (result.IsFailed) throw new Exception(string.Join(";", result.Errors.Select(x => x.Message)));

        return result;
    }

    public static Result ThrowExceptionIfHasFailedResult(this Result result)
    {
        if (result.IsFailed) throw new Exception(string.Join(";", result.Errors.Select(x => x.Message)));

        return result;
    }
}