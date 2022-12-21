namespace TransactionMobile.Maui;

//using Android.Media;
using System.Diagnostics;
using System.Windows.Input;
using BusinessLogic.UIServices;
using Newtonsoft.Json;
using Pages;
using TransactionMobile.Maui.UIServices;
using Shell = Microsoft.Maui.Controls.Shell;

public partial class AppShell : Shell
{
	public AppShell()
	{
        Console.WriteLine("In AppShell Ctor");
        InitializeComponent();
	}

    protected override async void OnNavigating(ShellNavigatingEventArgs args) {
        base.OnNavigating(args);
        
        Shared.Logger.Logger.LogDebug($"In OnNavigating - Source [{args.Source.ToString()}] {JsonConvert.SerializeObject(args)}");
        if (args.Source == ShellNavigationSource.ShellSectionChanged) {
            List<Page> existingPages = Navigation.NavigationStack.ToList();
            foreach (Page page in existingPages)
            {
                if (page is (LoginPage))
                    continue;
                
                if (page != null) {
                    Navigation.RemovePage(page);
                }
            }
        }
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args) {
        Shared.Logger.Logger.LogDebug($"In OnNavigated - Source [{args.Source.ToString()}] {JsonConvert.SerializeObject(args)}");
        
        base.OnNavigated(args);
    }
}
