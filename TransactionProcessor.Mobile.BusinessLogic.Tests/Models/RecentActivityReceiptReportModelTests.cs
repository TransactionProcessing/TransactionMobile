using System.Reflection;
using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ReportModelTests;

public class RecentActivityReceiptReportModelTests
{
    [Fact]
    public void RecentActivityReceiptReportModel_DefinesSharedSuccessStatusConstant()
    {
        FieldInfo? field = typeof(RecentActivityReceiptReportModel).GetField("SuccessStatus", BindingFlags.NonPublic | BindingFlags.Static);

        field.ShouldNotBeNull();
        field!.IsLiteral.ShouldBeTrue();
        field.GetRawConstantValue().ShouldBe("Success");
    }
}
