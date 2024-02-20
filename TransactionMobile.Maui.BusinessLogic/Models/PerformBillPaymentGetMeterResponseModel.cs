namespace TransactionMobile.Maui.BusinessLogic.Models;

using System.Diagnostics.CodeAnalysis;
using ViewModels.Transactions;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetMeterResponseModel
{
    public Boolean IsSuccessful { get; set; }

    public MeterDetails MeterDetails { get; set; }
}