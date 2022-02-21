namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Models;

    public interface ITransactionService
    {
        #region Methods

        Task<Boolean> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Boolean> PerformReconciliation(CancellationToken cancellationToken);

        Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        #endregion
    }

    public class DummyTransactionService : ITransactionService
    {
        public async Task<Boolean> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken)
        {
            return true;
        }

        public async Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken)
        {
            if (model.TopupAmount == 100)
            {
                return false;
            }

            return true;
        }

        public async Task<Boolean> PerformReconciliation(CancellationToken cancellationToken)
        {
            return true;
        }

        public async Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken)
        {
            if (model.VoucherAmount == 100)
            {
                return false;
            }

            return true;
        }
    }
}