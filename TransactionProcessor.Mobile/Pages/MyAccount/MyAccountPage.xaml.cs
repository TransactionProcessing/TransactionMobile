using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.MyAccount;

public partial class MyAccountPage : NoBackWithoutLogoutPage
{
    private MyAccountPageViewModel viewModel => BindingContext as MyAccountPageViewModel;

    public MyAccountPage(MyAccountPageViewModel vm)
    {
        this.InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadOptions(this.viewModel);
    }

    private void LoadOptions(MyAccountPageViewModel viewModel)
    {
        this.MyAccountOptionsList.Clear();

        Int32 rowCount = 0;
        foreach (ListViewItem modelOption in viewModel.MyAccountOptions)
        {
            Frame tile = this.CreateTile(modelOption.Title, (Color)Application.Current.Resources["profile"], $"{modelOption.Title.Replace(" ", "")}Button");
            TapGestureRecognizer tap = new TapGestureRecognizer
            {
                Command = new Command(() => viewModel.OptionSelectedCommand.Execute(
                    new ItemSelected<ListViewItem> { SelectedItem = modelOption, SelectedItemIndex = rowCount }))
            };
            tile.GestureRecognizers.Add(tap);
            this.MyAccountOptionsList.Add(tile);
            rowCount++;
        }
    }

    private Frame CreateTile(string text, Color backgroundColor, string automationId)
    {
        return new Frame
        {
            CornerRadius = 14,
            HasShadow = true,
            Padding = new Thickness(14),
            HeightRequest = 56,
            BackgroundColor = backgroundColor,
            BorderColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            AutomationId = automationId,
            Content = new Label
            {
                Text = text,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            }
        };
    }
}
