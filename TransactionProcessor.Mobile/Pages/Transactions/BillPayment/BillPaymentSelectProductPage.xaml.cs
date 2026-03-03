using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentSelectProductPage : ContentPage
{
    private BillPaymentSelectProductPageViewModel viewModel => this.BindingContext as BillPaymentSelectProductPageViewModel;

public BillPaymentSelectProductPage(BillPaymentSelectProductPageViewModel vm)
{
this.InitializeComponent();
        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadProducts(this.viewModel);
    }

    private void LoadProducts(BillPaymentSelectProductPageViewModel viewModel)
    {
        this.ProductsList.Children.Clear();
        this.ProductsList.RowDefinitions.Clear();

        var tiles = new List<Frame>();
        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products)
        {
            Frame tile = this.CreateTile(modelProduct.ProductDisplayText, (Color)Application.Current.Resources["billPayment"], modelProduct.ProductDisplayText.Replace(" ", ""));
            TapGestureRecognizer tap = new TapGestureRecognizer
            {
                Command = new Command(() => viewModel.ProductSelectedCommand.Execute(
                    new ItemSelected<ContractProductModel> { SelectedItem = modelProduct, SelectedItemIndex = rowCount }))
            };
            tile.GestureRecognizers.Add(tap);
            tiles.Add(tile);
            rowCount++;
        }

        int row = 0;
        for (int i = 0; i < tiles.Count; i += 2)
        {
            this.ProductsList.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(tiles[i], row);
            Grid.SetColumn(tiles[i], 0);
            this.ProductsList.Children.Add(tiles[i]);
            if (i + 1 < tiles.Count)
            {
                Grid.SetRow(tiles[i + 1], row);
                Grid.SetColumn(tiles[i + 1], 1);
                this.ProductsList.Children.Add(tiles[i + 1]);
            }
            row++;
        }

        Frame backTile = this.CreateBackTile(viewModel.BackButtonCommand);
        this.ProductsList.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        Grid.SetRow(backTile, row);
        Grid.SetColumn(backTile, 0);
        Grid.SetColumnSpan(backTile, 2);
        this.ProductsList.Children.Add(backTile);
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
            Content = new Label
            {
                Text = text,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                AutomationId = automationId
            }
        };
    }

    private Frame CreateBackTile(System.Windows.Input.ICommand command)
    {
        Frame frame = this.CreateTile("Back", (Color)Application.Current.Resources["MidGray"], "BackButton");
        frame.GestureRecognizers.Add(new TapGestureRecognizer { Command = command });
        return frame;
    }
}
