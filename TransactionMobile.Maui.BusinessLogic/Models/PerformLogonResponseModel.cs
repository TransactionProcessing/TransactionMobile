namespace TransactionMobile.Maui.BusinessLogic.Models;

using ViewModels.Transactions;

public class PerformLogonResponseModel
{
    #region Properties
    public String ResponseCode { get; set; }
    public String ResponseMessage { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }

    public Boolean RequireApplicationUpdate { get; set; }

    #endregion
}

public class PerformBillPaymentGetAccountResponseModel
{
    public Boolean IsSuccessful { get; set; }

    public BillDetails BillDetails{ get; set; }
}

public static class Extenstions
{
    public static Boolean IsSuccessful(this PerformLogonResponseModel model) {
        return model.ResponseCode == "0000";
    }
}