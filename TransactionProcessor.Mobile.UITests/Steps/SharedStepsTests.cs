using Reqnroll;
using Shouldly;
using TransactionProcessor.Mobile.UiTestBackend;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Steps;

[TestFixture]
public class SharedStepsTests
{
    [Test]
    public async Task GivenTheFollowingTransactionMixReportTransactionsExist_MapsEstablishedLabelsToRequestKinds()
    {
        TestingContext testingContext = new();
        testingContext.TestHostHelper = new TestHostHelper(testingContext);
        SharedSteps sut = new(testingContext);

        Table table = new(["Reference", "TransactionType", "Product", "Operator", "Status", "Amount", "TransactionDateTime"]);
        table.AddRow(["TXN-10001", "Mobile Topup", "Custom", "Safaricom", "Success", "100.00", "Today"]);
        table.AddRow(["TXN-10002", "Bill Payment", "Bill Pay (Post)", "PataPawa PostPay", "Success", "250.00", "Today"]);
        table.AddRow(["TXN-10003", "Voucher Issue", "10 KES", "Voucher", "Failed", "0.00", "Today"]);

        DataTable dataTable = DataTable.FromTable(table);

        await sut.GivenTheFollowingTransactionMixReportTransactionsExist(dataTable);

        testingContext.ScenarioSeed.ReportTransactions.Count.ShouldBe(3);
        testingContext.ScenarioSeed.ReportTransactions[0].TransactionType.ShouldBe(RequestKind.MobileTopup);
        testingContext.ScenarioSeed.ReportTransactions[1].TransactionType.ShouldBe(RequestKind.BillPaymentMakePayment);
        testingContext.ScenarioSeed.ReportTransactions[2].TransactionType.ShouldBe(RequestKind.Voucher);
    }
}
