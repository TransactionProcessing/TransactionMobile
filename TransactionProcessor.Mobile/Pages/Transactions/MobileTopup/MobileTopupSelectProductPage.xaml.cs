using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;

public partial class MobileTopupSelectProductPage : ContentPage
{
    #region Constructors

    public MobileTopupSelectProductPage(MobileTopupSelectProductPageViewModel vm) {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    #endregion

    #region Properties

    private MobileTopupSelectProductPageViewModel viewModel => this.BindingContext as MobileTopupSelectProductPageViewModel;

    #endregion

    #region Methods

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadProducts(this.viewModel);
    }

    private void LoadProducts(MobileTopupSelectProductPageViewModel viewModel) {
        this.ProductsList.Clear();

        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products) {
            Frame tile = this.CreateTile(modelProduct.ProductDisplayText, (Color)Application.Current.Resources["mobileTopup"], modelProduct.ProductDisplayText);
            TapGestureRecognizer tap = new TapGestureRecognizer
            {
                Command = new Command(() => viewModel.ProductSelectedCommand.Execute(
                    new ItemSelected<ContractProductModel> { SelectedItem = modelProduct, SelectedItemIndex = rowCount }))
            };
            tile.GestureRecognizers.Add(tap);
            this.ProductsList.Add(tile);
            rowCount++;
        }

        this.ProductsList.Add(this.CreateBackTile(viewModel.BackButtonCommand));
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

    private Frame CreateBackTile(System.Windows.Input.ICommand command)
    {
        Frame frame = this.CreateTile("Back", (Color)Application.Current.Resources["MidGray"], "BackButton");
        frame.GestureRecognizers.Add(new TapGestureRecognizer { Command = command });
        return frame;
    }

    #endregion
}
