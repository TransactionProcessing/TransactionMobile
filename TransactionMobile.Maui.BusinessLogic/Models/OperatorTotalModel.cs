namespace TransactionMobile.Maui.BusinessLogic.Models;

public class OperatorTotalModel
{
    public Guid ContractId { get; set; }
    public String OperatorIdentifier { get; set; }
    public Decimal TransactionValue { get; set; }
    public Int32 TransactionCount { get; set; }
}