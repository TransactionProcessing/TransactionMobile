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
        this.MyAccountOptionsList.Children.Clear();
        this.MyAccountOptionsList.RowDefinitions.Clear();

        var tiles = new List<Frame>();
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
            tiles.Add(tile);
            rowCount++;
        }

        int row = 0;
        for (int i = 0; i < tiles.Count; i += 2)
        {
            this.MyAccountOptionsList.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(tiles[i], row);
            Grid.SetColumn(tiles[i], 0);
            this.MyAccountOptionsList.Children.Add(tiles[i]);
            if (i + 1 < tiles.Count)
            {
                Grid.SetRow(tiles[i + 1], row);
                Grid.SetColumn(tiles[i + 1], 1);
                this.MyAccountOptionsList.Children.Add(tiles[i + 1]);
            }
            row++;
        }
    }

    private Frame CreateTile(string text, Color backgroundColor, string automationId)
    {
        return new Frame
        {
            CornerRadius = 16,
            HasShadow = true,
            Padding = new Thickness(12),
            HeightRequest = 100,
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
