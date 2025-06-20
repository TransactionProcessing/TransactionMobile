using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class PerformBillPaymentMakePostPaymentRequest : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>
{
    #region Constructors

    private PerformBillPaymentMakePostPaymentRequest(DateTime transactionDateTime,
                                                     Guid contractId,
                                                     Guid productId,
                                                     Guid operatorId,
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
        this.OperatorId = operatorId;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String CustomerAccountNumber { get; }

    public String CustomerAccountName { get; }

    public String CustomerMobileNumber { get; }

    public Decimal PaymentAmount { get; }

    public Guid OperatorId { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    #endregion

    #region Methods

    public static PerformBillPaymentMakePostPaymentRequest Create(DateTime transactionDateTime,
                                                                  Guid contractId,
                                                                  Guid productId,
                                                                  Guid operatorId,
                                                                  String customerAccountNumber,
                                                                  String customerAccountName,
                                                                  String customerMobileNumber,
                                                                  Decimal paymentAmount)
    {
        return new PerformBillPaymentMakePostPaymentRequest(transactionDateTime,
                                                            contractId,
                                                            productId,
                                                            operatorId,
                                                            customerAccountNumber,
                                                            customerAccountName,
                                                            customerMobileNumber,
                                                            paymentAmount);
    }

    #endregion
}