﻿using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class PerformBillPaymentGetAccountRequest : IRequest<Result<PerformBillPaymentGetAccountResponseModel>>
{
    #region Constructors

    private PerformBillPaymentGetAccountRequest(DateTime transactionDateTime,
                                                Guid contractId,
                                                Guid productId,
                                                Guid operatorId,
                                                String customerAccountNumber) {
        this.CustomerAccountNumber = customerAccountNumber;
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorId = operatorId;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public Guid OperatorId { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }
    
    #endregion

    #region Methods

    public static PerformBillPaymentGetAccountRequest Create(DateTime transactionDateTime,
                                                             Guid contractId,
                                                             Guid productId,
                                                             Guid operatorId,
                                                             String customerAccountNumber) {
        return new PerformBillPaymentGetAccountRequest(transactionDateTime,
                                                       contractId,
                                                       productId,
                                                       operatorId,
                                                       customerAccountNumber);
    }

    #endregion
}