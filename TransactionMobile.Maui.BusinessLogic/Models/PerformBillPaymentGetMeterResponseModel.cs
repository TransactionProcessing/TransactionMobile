namespace TransactionMobile.Maui.BusinessLogic.Models;

using ViewModels.Transactions;

public class PerformBillPaymentGetMeterResponseModel
{
    public Boolean IsSuccessful { get; set; }

    public MeterDetails MeterDetails { get; set; }
}