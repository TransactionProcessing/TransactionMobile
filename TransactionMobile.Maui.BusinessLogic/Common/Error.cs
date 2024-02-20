namespace TransactionMobile.Maui.BusinessLogic.Common;

public class Error
{
    public Error(string details) : this(null, details)
    {

    }

    public Error(string code, string details)
    {
        this.Code = code;
        this.Details = details;
    }

    public string Code { get; }
    public string Details { get; }
}