namespace TransactionMobile.Maui.BusinessLogic.Common;

public class ErrorResult<T> : Result<T>, IErrorResult
{
    public ErrorResult(string message) : this(message, Array.Empty<Error>())
    {
        this.Message = message;
        Success = false;
    }

    public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default)
    {
            
    }

    public string Message { get; set; }
}