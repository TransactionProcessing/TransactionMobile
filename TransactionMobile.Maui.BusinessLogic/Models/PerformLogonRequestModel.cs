namespace TransactionMobile.Maui.BusinessLogic.Models;

public class PerformLogonRequestModel
{
    #region Properties

    public String ApplicationVersion { get; set; }

    public String DeviceIdentifier { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public String TransactionNumber { get; set; }

    #endregion
}