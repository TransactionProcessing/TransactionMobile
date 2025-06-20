using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class SalesAnalysisPage : BasePage2
{
    protected override String Trait
    {
        get
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                return "Sales Analysis";
            }
            return "Sales Analysis";
        }
    }
    
    public SalesAnalysisPage(TestingContext testingContext) : base(testingContext)
    {
        
    }
}