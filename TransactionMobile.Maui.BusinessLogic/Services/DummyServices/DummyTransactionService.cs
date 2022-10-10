namespace TransactionMobile.Maui.BusinessLogic.Services.DummyServices;

using Models;
using ViewModels.Transactions;

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

    public async Task<PerformBillPaymentGetAccountResponseModel> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model,
                                                                                        CancellationToken cancellationToken) {
        if (model.CustomerAccountNumber == "123456") {
            return new PerformBillPaymentGetAccountResponseModel {
                                                                     BillDetails = null,
                                                                     IsSuccessful = false
                                                                 };
        }

        return new PerformBillPaymentGetAccountResponseModel {
                                                                 BillDetails = new BillDetails {
                                                                                                   AccountName = "Mr Test Customer",
                                                                                                   AccountNumber = model.CustomerAccountNumber,
                                                                                                   Balance = "100.00",
                                                                                                   DueDate = DateTime.Now.AddDays(3).ToString("dd-MM-yyyy"),

                                                                                               },
                                                                 IsSuccessful = true
                                                             };
    }

    public async Task<Boolean> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                       CancellationToken cancellationToken) {

        if (model.PaymentAmount== 99.99m)
        {
            return false;
        }
        return true;
    }

    #endregion
}