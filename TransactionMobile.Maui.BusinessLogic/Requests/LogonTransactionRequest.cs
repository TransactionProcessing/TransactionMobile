﻿namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using SimpleResults;

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