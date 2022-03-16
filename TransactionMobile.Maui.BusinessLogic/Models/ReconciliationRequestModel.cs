namespace TransactionMobile.Maui.BusinessLogic.Models;

public class PerformReconciliationRequestModel
{
    public List<OperatorTotalModel> OperatorTotals { get; set; }
    public Decimal TransactionValue { get; set; }
    public Int32 TransactionCount { get; set; }
    public String DeviceIdentifier { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public String ApplicationVersion { get; set; }
}