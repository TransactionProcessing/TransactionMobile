using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformVoucherIssueRequestModel
{
    // TODO: should we have a base transaction request model ?

    #region Properties

    public String ApplicationVersion { get; set; }

    public Guid ContractId { get; set; }

    public String RecipientMobileNumber { get; set; }
    public String RecipientEmailAddress { get; set; }

    public String CustomerEmailAddress { get; set; }

    public String DeviceIdentifier { get; set; }

    public String OperatorIdentifier { get; set; }

    public Guid ProductId { get; set; }

    public Decimal VoucherAmount { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public String TransactionNumber { get; set; }

    #endregion
}