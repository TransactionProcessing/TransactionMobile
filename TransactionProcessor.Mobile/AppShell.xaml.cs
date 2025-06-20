using Microsoft.Maui.Controls;
using TransactionProcessor.Mobile.Pages;

namespace TransactionProcessor.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            //Logger.LogDebug($"In OnNavigating - Source [{args.Source.ToString()}] {JsonConvert.SerializeObject(args)}");
            if (args.Source == ShellNavigationSource.ShellSectionChanged)
            {
                List<Page> existingPages = Navigation.NavigationStack.ToList();
                foreach (Page page in existingPages)
                {
                    if (page is (LoginPage))
                        continue;

                    if (page != null)
                    {
                        Navigation.RemovePage(page);
                    }
                }
            }
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            //Logger.LogDebug($"In OnNavigated - Source [{args.Source.ToString()}] {JsonConvert.SerializeObject(args)}");

            base.OnNavigated(args);
        }
    }
}
