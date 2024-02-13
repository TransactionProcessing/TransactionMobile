namespace TransactionMobile.Maui.BusinessLogic.Models;

public class MerchantDetailsModel
{
    public Decimal Balance { get; set; }
    public Decimal AvailableBalance { get; set; }
    public String MerchantName { get; set; }
    public DateTime NextStatementDate { get; set; }
    public DateTime LastStatementDate { get; set; }
    public String SettlementSchedule { get; set; }
    public AddressModel Address { get; set; }
    public ContactModel Contact { get; set; }
}