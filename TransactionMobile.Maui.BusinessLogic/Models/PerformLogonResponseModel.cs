namespace TransactionMobile.Maui.BusinessLogic.Models;

public class PerformLogonResponseModel
{
    #region Properties
    public String ResponseCode { get; set; }
    public String ResponseMessage { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }

    public Boolean IsSuccessful => ResponseCode == "0000";

    #endregion
}