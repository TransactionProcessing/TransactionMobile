namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using Common;
using Drivers;
using UITests;

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