using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class PerformBillPaymentMakePaymentModel
{
    #region Properties

    public String ApplicationVersion { get; set; }

    public Guid ContractId { get; set; }

    public String CustomerAccountNumber { get; set; }

    public String CustomerMobileNumber { get; set; }

    public String CustomerAccountName { get; set; }

    public Decimal PaymentAmount { get; set; }

    public String DeviceIdentifier { get; set; }

    public Guid OperatorId { get; set; }

    public Guid ProductId { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public String TransactionNumber { get; set; }

    public Boolean PostPayment{ get; set; }

    #endregion
}