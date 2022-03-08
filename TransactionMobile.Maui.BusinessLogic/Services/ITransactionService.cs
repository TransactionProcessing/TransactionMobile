namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Models;

    public interface ITransactionService
    {
        #region Methods

        Task<PerformLogonResponseModel> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformReconciliation(CancellationToken cancellationToken);

        Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        #endregion
    }
}