namespace TransactionMobile.Maui;

//using Android.Media;
using System.Diagnostics;
using System.Windows.Input;
using Newtonsoft.Json;
using Shell = Microsoft.Maui.Controls.Shell;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

    protected override void OnNavigating(ShellNavigatingEventArgs args) {
        Shared.Logger.Logger.LogDebug($"In OnNavigating - Source [{args.Source.ToString()}] {JsonConvert.SerializeObject(args)}");
        if (args.Source == ShellNavigationSource.ShellSectionChanged) {
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
                if (page != null) {
                    Navigation.RemovePage(page);
                }
            }
        }
        base.OnNavigating(args);
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args) {
        Shared.Logger.Logger.LogDebug($"In OnNavigated - Source [{args.Source.ToString()}] {JsonConvert.SerializeObject(args)}");
        base.OnNavigated(args);
    }
}
