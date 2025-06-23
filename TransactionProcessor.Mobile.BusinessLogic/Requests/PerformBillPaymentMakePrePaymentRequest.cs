using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class PerformBillPaymentMakePrePaymentRequest : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>
{
    #region Constructors

    private PerformBillPaymentMakePrePaymentRequest(DateTime transactionDateTime,
                                                    Guid contractId,
                                                    Guid productId,
                                                    Guid operatorId,
                                                    String meterNumber,
                                                    String customerAccountName,
                                                    Decimal paymentAmount)
    {
        this.CustomerAccountName = customerAccountName;
        this.MeterNumber = meterNumber;
        this.PaymentAmount = paymentAmount;
        this.TransactionDateTime = transactionDateTime;
        this.ContractId = contractId;
        this.ProductId = productId;
        this.OperatorId = operatorId;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String MeterNumber { get; }

    public String CustomerAccountName { get; }

    public Decimal PaymentAmount { get; }

    public Guid OperatorId { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    #endregion

    #region Methods

    public static PerformBillPaymentMakePrePaymentRequest Create(DateTime transactionDateTime,
                                                                 Guid contractId,
                                                                 Guid productId,
                                                                 Guid operatorId,
                                                                 String meterNumber,
                                                                 String customerAccountName,
                                                                 Decimal paymentAmount)
    {
        return new PerformBillPaymentMakePrePaymentRequest(transactionDateTime,
            contractId,
            productId,
            operatorId,
            meterNumber,
            customerAccountName,
            paymentAmount);
    }

    #endregion
}