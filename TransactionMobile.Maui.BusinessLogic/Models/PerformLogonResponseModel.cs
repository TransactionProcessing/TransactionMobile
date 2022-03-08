namespace TransactionMobile.Maui.BusinessLogic.Models;

public class PerformLogonResponseModel
{
    #region Properties
    
    public Boolean IsSuccessful { get; set; }

    public String ResponseMessage { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }

    public Boolean RequireApplicationUpdate { get; set; }

    #endregion
}