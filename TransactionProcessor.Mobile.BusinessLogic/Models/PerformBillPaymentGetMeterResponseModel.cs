using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetMeterResponseModel
{
    public Boolean IsSuccessful => this.MeterDetails != null;

    public MeterDetails MeterDetails { get; set; }
}