namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using SimpleResults;
using TransactionProcessorACL.DataTransferObjects.Responses;

public class PerformBillPaymentMakePostPaymentRequest : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>
{
    #region Constructors

    private PerformBillPaymentMakePostPaymentRequest(DateTime transactionDateTime,
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

    public static PerformBillPaymentMakePostPaymentRequest Create(DateTime transactionDateTime,
                                                                  Guid contractId,
                                                                  Guid productId,
                                                                  String operatorIdentifier,
                                                                  String customerAccountNumber,
                                                                  String customerAccountName,
                                                                  String customerMobileNumber,
                                                                  Decimal paymentAmount)
    {
        return new PerformBillPaymentMakePostPaymentRequest(transactionDateTime,
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

public class PerformBillPaymentMakePrePaymentRequest : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>
{
    #region Constructors

    private PerformBillPaymentMakePrePaymentRequest(DateTime transactionDateTime,
                                                    Guid contractId,
                                                    Guid productId,
                                                    String operatorIdentifier,
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
        this.OperatorIdentifier = operatorIdentifier;
    }

    #endregion

    #region Properties

    public Guid ContractId { get; }

    public String MeterNumber { get; }

    public String CustomerAccountName { get; }

    public Decimal PaymentAmount { get; }

    public String OperatorIdentifier { get; }

    public Guid ProductId { get; }

    public DateTime TransactionDateTime { get; }

    #endregion

    #region Methods

    public static PerformBillPaymentMakePrePaymentRequest Create(DateTime transactionDateTime,
                                                                 Guid contractId,
                                                                 Guid productId,
                                                                 String operatorIdentifier,
                                                                 String meterNumber,
                                                                 String customerAccountName,
                                                                 Decimal paymentAmount)
    {
        return new PerformBillPaymentMakePrePaymentRequest(transactionDateTime,
                                                           contractId,
                                                           productId,
                                                           operatorIdentifier,
                                                           meterNumber,
                                                           customerAccountName,
                                                           paymentAmount);
    }

    #endregion
}