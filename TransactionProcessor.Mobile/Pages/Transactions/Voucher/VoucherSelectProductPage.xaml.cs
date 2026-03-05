using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.Voucher;

public partial class VoucherSelectProductPage : ContentPage
{
    private VoucherSelectProductPageViewModel viewModel => this.BindingContext as VoucherSelectProductPageViewModel;

	public VoucherSelectProductPage(VoucherSelectProductPageViewModel vm)
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

    private void LoadProducts(VoucherSelectProductPageViewModel viewModel)
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
        tile.SetDynamicResource(VisualElement.StyleProperty, "OperatorTileFrame");
        tile.AutomationId = modelProduct.ProductDisplayText;

        Image icon = new Image
        {
            Source = "transactionsbutton.svg",
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
        nameLabel.SetDynamicResource(Label.TextColorProperty, "voucher");

        tile.Content = new VerticalStackLayout
        {
            Spacing = 8,
            HorizontalOptions = LayoutOptions.Center,
            Children = { icon, nameLabel }
        };

        Binding commandParameter = new Binding
        {
            Source = new ItemSelected<ContractProductModel>
            {
                SelectedItem = modelProduct,
                SelectedItemIndex = rowCount
            }
        };

        Binding command = new Binding("ProductSelectedCommand", source: this.viewModel);

        TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, command);
        tapGesture.SetBinding(TapGestureRecognizer.CommandParameterProperty, commandParameter);

        tile.GestureRecognizers.Add(tapGesture);

        return tile;
    }
}