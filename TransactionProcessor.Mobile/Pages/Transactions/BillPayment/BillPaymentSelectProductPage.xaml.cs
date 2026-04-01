using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using TransactionProcessor.Mobile.Pages.Common;

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

        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products)
        {
            Frame tile = this.CreateProductTile(modelProduct, rowCount);
            this.ProductsList.Children.Add(tile);
            rowCount++;
        }
    }

    private Frame CreateProductTile(ContractProductModel modelProduct, Int32 rowCount)
    {
        Frame tile = new Frame();
        tile.SetDynamicResource(VisualElement.StyleProperty, "SelectionTileFrame");
        tile.AutomationId = modelProduct.ProductDisplayText.Replace(" ", "");

        Image icon = new Image
        {
            Source = ThemeButtonImageSource.Get("transactionsbutton.svg"),
            HeightRequest = 36,
            WidthRequest = 36,
            HorizontalOptions = LayoutOptions.Center
        };

        Label nameLabel = new Label
        {
            Text = modelProduct.ProductDisplayText,
            HorizontalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 13
        };
        nameLabel.SetDynamicResource(Label.TextColorProperty, "billPayment");

        tile.Content = new VerticalStackLayout
        {
            Spacing = 8,
            HorizontalOptions = LayoutOptions.Center,
            Children = { icon, nameLabel }
        };

        Binding commandParameter = new Binding { Source = new ItemSelected<ContractProductModel>() { SelectedItem = modelProduct, SelectedItemIndex = rowCount } };
        Binding command = new Binding("ProductSelectedCommand", source: this.viewModel);

        TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, command);
        tapGesture.SetBinding(TapGestureRecognizer.CommandParameterProperty, commandParameter);

        tile.GestureRecognizers.Add(tapGesture);

        return tile;
    }
}
