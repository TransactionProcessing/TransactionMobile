namespace TransactionMobile.Maui.BusinessLogic.Services.DummyServices;

using Models;

public class DummyTransactionService : ITransactionService
{
    #region Methods

    public async Task<PerformLogonResponseModel> PerformLogon(PerformLogonRequestModel model,
                                                              CancellationToken cancellationToken)
    {
        return new PerformLogonResponseModel
               {
                   EstateId = Guid.Parse("D7E52254-E0BE-436A-9A34-CC291DA0D66A"),
                   MerchantId = Guid.Parse("DD034A3B-D8EE-45A4-A29F-8774751CEE76"),
                   IsSuccessful = true,
                   ResponseMessage = "SUCCESS",
                   RequireApplicationUpdate = false
               };
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

    public async Task<Boolean> PerformReconciliation(PerformReconciliationRequestModel model,CancellationToken cancellationToken)
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