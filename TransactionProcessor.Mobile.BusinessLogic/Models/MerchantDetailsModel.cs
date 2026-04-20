using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class MerchantDetailsModel
{
    public Guid EstateId { get; set; }
    public Guid MerchantId { get; set; }
    public Decimal Balance { get; set; }
    public Decimal AvailableBalance { get; set; }
    public String MerchantName { get; set; }
    public DateTime NextStatementDate { get; set; }
    public DateTime LastStatementDate { get; set; }
    public String SettlementSchedule { get; set; }
    public AddressModel Address { get; set; }
    public ContactModel Contact { get; set; }
}