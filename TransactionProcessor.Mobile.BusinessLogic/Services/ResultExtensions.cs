using TransactionProcessing.UnionResults;

public static class ResultExtensions
{
    public static Result<T> FailureExtended<T>(string message, Exception exception)
    {
        return Result<T>.Fail(Result.Failure(message, new[] { exception.Message }));
    }
}
