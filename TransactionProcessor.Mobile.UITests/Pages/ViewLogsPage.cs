using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class ViewLogsPage : BasePage2{
    public ViewLogsPage(TestingContext testingContext) : base(testingContext){
        
    }

    protected override String Trait => "ViewLogs";
}