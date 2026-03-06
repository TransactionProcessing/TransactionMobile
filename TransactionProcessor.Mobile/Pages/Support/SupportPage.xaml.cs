using System.Windows.Input;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.Support
{
    public partial class SupportPage : NoBackWithoutLogoutPage
    {
        private SupportPageViewModel viewModel => BindingContext as SupportPageViewModel;

        public SupportPage(SupportPageViewModel vm) {
            this.InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            this.LoadSupportOptions(this.viewModel);
        }

        private void LoadSupportOptions(SupportPageViewModel viewModel) {
            this.SupportOptionsList.Children.Clear();

            var options = new List<(String Title, String Icon, String AutomationId, ICommand Command)>
            {
                ("Upload Logs", "supportbutton.svg", "UploadLogsButton", viewModel.UploadLogsCommand),
                ("View Logs",   "reportbutton.svg",  "ViewLogsButton",   viewModel.ViewLogsCommand),
            };

            foreach (var option in options) {
                Frame tile = this.CreateSupportTile(option.Title, option.Icon, option.AutomationId, option.Command);
                this.SupportOptionsList.Children.Add(tile);
            }
        }

        private Frame CreateSupportTile(String title, String iconSource, String automationId, ICommand command) {
            Frame tile = new Frame();
            tile.SetDynamicResource(VisualElement.StyleProperty, "SelectionTileFrame");
            tile.AutomationId = automationId;

            Image icon = new Image
            {
                Source = iconSource,
                HeightRequest = 36,
                WidthRequest = 36,
                HorizontalOptions = LayoutOptions.Center
            };

            Label nameLabel = new Label
            {
                Text = title,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 13
            };
            nameLabel.SetDynamicResource(Label.TextColorProperty, "support");

            tile.Content = new VerticalStackLayout
            {
                Spacing = 8,
                HorizontalOptions = LayoutOptions.Center,
                Children = { icon, nameLabel }
            };

            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Command = command;
            tile.GestureRecognizers.Add(tapGesture);

            return tile;
        }
    }
}