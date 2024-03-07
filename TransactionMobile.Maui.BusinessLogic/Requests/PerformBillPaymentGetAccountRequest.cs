namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using SimpleResults;

public class PerformBillPaymentGetAccountRequest : IRequest<Result<PerformBillPaymentGetAccountResponseModel>>
{
    #region Constructors

    private PerformBillPaymentGetAccountRequest(DateTime transactionDateTime,
                                                Guid contractId,
                                                Guid productId,
                                                String operatorIdentifier,
                                                String customerAccountNumber) {
        this.CustomerAccountNumber = customerAccountNumber;
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorIdentifier = operatorIdentifier;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }
    
    #endregion

    #region Methods

    public static PerformBillPaymentGetAccountRequest Create(DateTime transactionDateTime,
                                                             Guid contractId,
                                                             Guid productId,
                                                             String operatorIdentifier,
                                                             String customerAccountNumber) {
        return new PerformBillPaymentGetAccountRequest(transactionDateTime,
                                                       contractId,
                                                       productId,
                                                       operatorIdentifier,
                                                       customerAccountNumber);
    }

    #endregion
}