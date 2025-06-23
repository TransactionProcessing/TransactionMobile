using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetAccountModel
{
    #region Properties

    public String ApplicationVersion { get; set; }

    public Guid ContractId { get; set; }
    
    public String CustomerAccountNumber { get; set; }

    public String DeviceIdentifier { get; set; }

    public Guid OperatorId { get; set; }

    public Guid ProductId { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public String TransactionNumber { get; set; }

    #endregion
}