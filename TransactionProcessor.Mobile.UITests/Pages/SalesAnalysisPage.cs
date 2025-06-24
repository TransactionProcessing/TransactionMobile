using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class SalesAnalysisPage : BasePage2
{
    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform switch
    {
        MobileTestPlatform.iOS => "Sales Analysis",
        _ => "SalesAnalysis"
    };

    public SalesAnalysisPage(TestingContext testingContext) : base(testingContext)
    {
        
    }
}