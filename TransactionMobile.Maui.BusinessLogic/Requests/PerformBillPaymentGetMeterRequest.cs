﻿namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;

public class PerformBillPaymentGetMeterRequest : IRequest<Result<PerformBillPaymentGetMeterResponseModel>>
{
    #region Constructors

    private PerformBillPaymentGetMeterRequest(DateTime transactionDateTime,
                                              Guid contractId,
                                              Guid productId,
                                              String operatorIdentifier,
                                              String meterNumber)
    {
        this.MeterNumber = meterNumber;
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorIdentifier = operatorIdentifier;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String MeterNumber { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }
    
    #endregion

    #region Methods

    public static PerformBillPaymentGetMeterRequest Create(DateTime transactionDateTime,
                                                           Guid contractId,
                                                           Guid productId,
                                                           String operatorIdentifier,
                                                           String meterNumber)
    {
        return new PerformBillPaymentGetMeterRequest(transactionDateTime,
                                                     contractId,
                                                     productId,
                                                     operatorIdentifier,
                                                     meterNumber);
    }

    #endregion
}