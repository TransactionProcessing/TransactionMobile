namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Models;
    using RequestHandlers;
    using TransactionProcessorACL.DataTransferObjects.Responses;

    public interface ITransactionService
    {
        #region Methods

        Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Result<SaleTransactionResponseMessage>> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Result<ReconciliationResponseMessage>> PerformReconciliation(PerformReconciliationRequestModel model, CancellationToken cancellationToken);

        Task<Result<SaleTransactionResponseMessage>> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model, CancellationToken cancellationToken);

        Task<Result<SaleTransactionResponseMessage>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model, CancellationToken cancellationToken);

        #endregion
    }
}