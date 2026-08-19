using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ModelTests;

public class AutomationIdHelperTests
{
    [Fact]
    public void Create_ReplacesUnsupportedCharactersWithUnderscores()
    {
        AutomationIdHelper.Create("RecentActivityResult", "TXN-10001")
            .ShouldBe("RecentActivityResult_TXN_10001");
    }

    [Fact]
    public void Create_TrimsAndCollapsesSeparators()
    {
        AutomationIdHelper.Create("TransactionMixItem", "  bill-pay-post  ")
            .ShouldBe("TransactionMixItem_bill_pay_post");
    }

    [Fact]
    public void Create_ReturnsPrefixForEmptyValues()
    {
        AutomationIdHelper.Create("TransactionMixItem", " ")
            .ShouldBe("TransactionMixItem");
    }
}
