using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentMakePaymentResponseModel
{
    public string ResponseCode { get; set; }

    public string ResponseMessage { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }
    public Boolean IsSuccessful => this.ResponseCode == "0000";
}