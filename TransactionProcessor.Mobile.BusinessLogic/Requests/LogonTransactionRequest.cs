using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class LogonTransactionRequest : IRequest<Result<PerformLogonResponseModel>>
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