using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class RecentActivityReceiptDetailPage : BasePage2
{
    protected override string Trait => AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows ? "Receipt Detail" : "ReceiptDetail";

    public RecentActivityReceiptDetailPage(TestingContext testingContext) : base(testingContext)
    {
    }
}
