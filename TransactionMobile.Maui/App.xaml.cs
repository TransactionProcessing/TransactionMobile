using TransactionMobile.Maui.Pages.Reports;
using TransactionMobile.Maui.Pages.Transactions;

namespace TransactionMobile.Maui;

using Pages.AppHome;

public partial class App : Application
{
	public App()
    {
        InitializeComponent();
		
		MainPage = new AppShell();

        Routing.RegisterRoute(nameof(MobileTopupSelectOperatorPage), typeof(MobileTopupSelectOperatorPage));
        Routing.RegisterRoute(nameof(MobileTopupSelectProductPage), typeof(MobileTopupSelectProductPage));
        Routing.RegisterRoute(nameof(MobileTopupPerformTopupPage), typeof(MobileTopupPerformTopupPage));
        Routing.RegisterRoute(nameof(MobileTopupSuccessPage), typeof(MobileTopupSuccessPage));
        Routing.RegisterRoute(nameof(MobileTopupFailedPage), typeof(MobileTopupFailedPage));
        Routing.RegisterRoute(nameof(VoucherSelectOperatorPage), typeof(VoucherSelectOperatorPage));
        Routing.RegisterRoute(nameof(VoucherSelectProductPage), typeof(VoucherSelectProductPage));
        Routing.RegisterRoute(nameof(VoucherPerformIssuePage), typeof(VoucherPerformIssuePage));
        Routing.RegisterRoute(nameof(VoucherIssueSuccessPage), typeof(VoucherIssueSuccessPage));
        Routing.RegisterRoute(nameof(VoucherIssueFailedPage), typeof(VoucherIssueFailedPage));
    }
}
