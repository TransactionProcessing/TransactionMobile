using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentGetMeterModel
{
    #region Properties

    public String ApplicationVersion { get; set; }

    public Guid ContractId { get; set; }

    public String MeterNumber { get; set; }

    public String DeviceIdentifier { get; set; }

    public Guid OperatorId { get; set; }

    public Guid ProductId { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public String TransactionNumber { get; set; }

    #endregion
}