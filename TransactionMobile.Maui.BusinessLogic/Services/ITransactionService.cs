namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Models;
    using RequestHandlers;

    public interface ITransactionService
    {
        #region Methods

        Task<PerformLogonResponseModel> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformReconciliation(PerformReconciliationRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        Task<PerformBillPaymentGetAccountResponseModel> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model, CancellationToken cancellationToken);

        #endregion
    }
}