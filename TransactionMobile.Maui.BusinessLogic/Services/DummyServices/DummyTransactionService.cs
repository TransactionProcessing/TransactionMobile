namespace TransactionMobile.Maui.BusinessLogic.Services.DummyServices;

using Models;

public class DummyTransactionService : ITransactionService
{
    #region Methods

    public async Task<Boolean> PerformLogon(PerformLogonRequestModel model,
                                            CancellationToken cancellationToken)
    {
        return true;
    }

    public async Task<Boolean> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                  CancellationToken cancellationToken)
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

    public async Task<Boolean> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                   CancellationToken cancellationToken)
    {
        if (model.VoucherAmount == 100)
        {
            return false;
        }

        return true;
    }

    #endregion
}