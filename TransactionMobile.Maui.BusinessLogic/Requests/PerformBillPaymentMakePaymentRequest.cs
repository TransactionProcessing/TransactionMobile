namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using TransactionProcessorACL.DataTransferObjects.Responses;

public class PerformBillPaymentMakePaymentRequest : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>
{
    #region Constructors

    private PerformBillPaymentMakePaymentRequest(DateTime transactionDateTime,
                                                 Guid contractId,
                                                 Guid productId,
                                                 String operatorIdentifier,
                                                 String customerAccountNumber,
                                                 String customerAccountName,
                                                 String customerMobileNumber,
                                                 Decimal paymentAmount)
    {
        this.CustomerAccountNumber = customerAccountNumber;
        this.CustomerAccountName = customerAccountName;
        this.CustomerMobileNumber = customerMobileNumber;
        this.PaymentAmount = paymentAmount;
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorIdentifier = operatorIdentifier;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public String CustomerAccountName { get; }

    public String CustomerMobileNumber { get; }

    public Decimal PaymentAmount { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    #endregion

    #region Methods

    public static PerformBillPaymentMakePaymentRequest Create(DateTime transactionDateTime,
                                                              Guid contractId,
                                                              Guid productId,
                                                              String operatorIdentifier,
                                                              String customerAccountNumber,
                                                              String customerAccountName,
                                                              String customerMobileNumber,
                                                              Decimal paymentAmount)
    {
        return new PerformBillPaymentMakePaymentRequest(transactionDateTime,
                                                        contractId,
                                                        productId,
                                                        operatorIdentifier,
                                                        customerAccountNumber,
                                                        customerAccountName,
                                                        customerMobileNumber,
                                                        paymentAmount);
    }

    #endregion
}