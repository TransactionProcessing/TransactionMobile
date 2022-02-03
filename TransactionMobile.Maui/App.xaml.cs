using TransactionMobile.Maui.Pages.Reports;
using TransactionMobile.Maui.Pages.Transactions;

namespace TransactionMobile.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		
		MainPage = new AppShell();	
	}
}
