namespace TransactionMobile.Maui.BusinessLogic.Common;

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}