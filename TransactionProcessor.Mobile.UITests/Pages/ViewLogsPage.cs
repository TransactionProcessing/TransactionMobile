using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class ViewLogsPage : BasePage2{
    public ViewLogsPage(TestingContext testingContext) : base(testingContext){
        
    }

    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform switch
    {
        MobileTestPlatform.iOS => "View Logs",
        _ => "ViewLogs"
    };
}