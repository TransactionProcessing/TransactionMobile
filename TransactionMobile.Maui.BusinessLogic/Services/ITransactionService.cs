namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Models;
    using RequestHandlers;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    public interface ITransactionService
    {
        #region Methods

        Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken);

        #endregion
    }
}