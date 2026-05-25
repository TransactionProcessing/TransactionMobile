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

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
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
    }
}
