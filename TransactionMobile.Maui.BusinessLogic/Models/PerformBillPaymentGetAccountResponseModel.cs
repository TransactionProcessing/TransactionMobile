namespace TransactionMobile.Maui.BusinessLogic.Models;

using System.Diagnostics.CodeAnalysis;
using ViewModels.Transactions;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetAccountResponseModel
{
    public Boolean IsSuccessful { get; set; }

    public BillDetails BillDetails{ get; set; }
}