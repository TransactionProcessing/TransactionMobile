namespace TransactionMobile.Maui.BusinessLogic.Models;

using System.Diagnostics.CodeAnalysis;
using ViewModels.Transactions;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetAccountResponseModel
{
    public Boolean IsSuccessful => this.BillDetails != null;

    public BillDetails BillDetails{ get; set; }
}