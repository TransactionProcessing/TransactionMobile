namespace TransactionMobile.Maui;

using System.Diagnostics;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

    protected override void OnNavigating(ShellNavigatingEventArgs args) {
        base.OnNavigating(args);
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args) {
        base.OnNavigated(args);
    }
}