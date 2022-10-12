namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using Models;

public class LogonTransactionRequest : IRequest<PerformLogonResponseModel>
{
    public DateTime TransactionDateTime { get; private set; }
    
    private LogonTransactionRequest(DateTime transactionDateTime)
    {
        this.TransactionDateTime=transactionDateTime;
    }

    public static LogonTransactionRequest Create(DateTime transactionDateTime)
    {
        return new LogonTransactionRequest(transactionDateTime);
    }

}