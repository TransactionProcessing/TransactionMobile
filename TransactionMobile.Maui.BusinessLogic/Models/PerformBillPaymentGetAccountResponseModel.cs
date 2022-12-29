namespace TransactionMobile.Maui.BusinessLogic.Models;

using ViewModels.Transactions;

public class PerformBillPaymentGetAccountResponseModel
{
    public Boolean IsSuccessful { get; set; }

    public BillDetails BillDetails{ get; set; }
}