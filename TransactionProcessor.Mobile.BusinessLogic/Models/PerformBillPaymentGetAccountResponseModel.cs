using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetAccountResponseModel
{
    public Boolean IsSuccessful => this.BillDetails != null;

    public BillDetails BillDetails{ get; set; }
}