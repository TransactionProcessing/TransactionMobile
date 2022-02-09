using TransactionMobile.Maui.Pages.Reports;
using TransactionMobile.Maui.Pages.Transactions;

namespace TransactionMobile.Maui;

using Pages.AppHome;

public partial class App : Application
{
    /// <summary>
    /// The estate identifier
    /// </summary>
    public static Guid EstateId;

    /// <summary>
    /// The merchant identifier
    /// </summary>
    public static Guid MerchantId;

	public App()
	{
		InitializeComponent();
		
		MainPage = new AppShell();

        Routing.RegisterRoute(nameof(MobileTopupSelectOperatorPage), typeof(MobileTopupSelectOperatorPage));
        Routing.RegisterRoute(nameof(MobileTopupSelectProductPage), typeof(MobileTopupSelectProductPage));
        Routing.RegisterRoute(nameof(MobileTopupPerformTopupPage), typeof(MobileTopupPerformTopupPage));
        Routing.RegisterRoute(nameof(MobileTopupSuccessPage), typeof(MobileTopupSuccessPage));
        Routing.RegisterRoute(nameof(MobileTopupFailedPage), typeof(MobileTopupFailedPage));
    }
}
