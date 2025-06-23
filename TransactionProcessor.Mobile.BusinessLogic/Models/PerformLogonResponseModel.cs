using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformLogonResponseModel
{
    #region Properties
    public String ResponseCode { get; set; }
    public String ResponseMessage { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }

    public Boolean IsSuccessful => this.ResponseCode == "0000";

    #endregion
}