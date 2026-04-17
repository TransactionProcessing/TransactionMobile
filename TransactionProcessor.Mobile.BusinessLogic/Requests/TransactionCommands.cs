using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record TransactionCommands {
    public record PerformMobileTopupCommand(DateTime TransactionDateTime, Guid ContractId, Guid ProductId, Guid OperatorId, String CustomerAccountNumber, Decimal TopupAmount, String CustomerEmailAddress) : IRequest<Result<PerformMobileTopupResponseModel>>;
    public record PerformVoucherIssueCommand(DateTime TransactionDateTime, Guid ContractId, Guid ProductId, Guid OperatorId, String RecipientMobileNumber, String RecipientEmailAddress, Decimal VoucherAmount, String CustomerEmailAddress) : IRequest<Result<PerformVoucherIssueResponseModel>>;
    public record PerformReconciliationCommand(DateTime TransactionDateTime, String DeviceIdentifier, String ApplicationVersion) : IRequest<Result<PerformReconciliationResponseModel>>;
    public record PerformBillPaymentGetAccountCommand(DateTime TransactionDateTime, Guid ContractId, Guid ProductId, Guid OperatorId, String CustomerAccountNumber) : IRequest<Result<PerformBillPaymentGetAccountResponseModel>>;
    public record PerformBillPaymentGetMeterCommand(DateTime TransactionDateTime, Guid ContractId, Guid ProductId, Guid OperatorId, String MeterNumber) : IRequest<Result<PerformBillPaymentGetMeterResponseModel>>;
    public record PerformBillPaymentMakePrePaymentCommand(DateTime TransactionDateTime, Guid ContractId, Guid ProductId, Guid OperatorId, String MeterNumber, String CustomerAccountName, Decimal PaymentAmount) : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>;
    public record PerformBillPaymentMakePostPaymentCommand(DateTime TransactionDateTime, Guid ContractId, Guid ProductId, Guid OperatorId, String CustomerAccountNumber, String CustomerAccountName, String CustomerMobileNumber, Decimal PaymentAmount) : IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>;
    public record PerformLogonCommand(DateTime TransactionDateTime) : IRequest<Result<PerformLogonResponseModel>>;
}