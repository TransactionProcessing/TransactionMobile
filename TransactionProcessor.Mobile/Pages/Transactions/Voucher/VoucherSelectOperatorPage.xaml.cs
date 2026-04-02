using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.Transactions.Voucher;

public partial class VoucherSelectOperatorPage : ContentPage
{
    private VoucherSelectOperatorPageViewModel viewModel => this.BindingContext as VoucherSelectOperatorPageViewModel;

	public VoucherSelectOperatorPage(VoucherSelectOperatorPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadOperators(this.viewModel);
    }

    private void LoadOperators(VoucherSelectOperatorPageViewModel viewModel)
    {
        this.OperatorList.Children.Clear();

        Int32 rowCount = 0;
        foreach (ContractOperatorModel modelOperator in viewModel.Operators)
        {
            Frame tile = this.CreateOperatorTile(modelOperator, rowCount);
            this.OperatorList.Children.Add(tile);
            rowCount++;
        }
    }

    private Frame CreateOperatorTile(ContractOperatorModel modelOperator, Int32 rowCount)
    {
        Frame tile = new Frame();
        tile.SetDynamicResource(VisualElement.StyleProperty, "SelectionTileFrame");
        tile.AutomationId = modelOperator.OperatorName.Replace(" ", "");

        Image icon = new Image
        {
            Source = ThemeButtonImageSource.Get("transactionsbutton.svg"),
            HeightRequest = 36,
            WidthRequest = 36,
            HorizontalOptions = LayoutOptions.Center
        };

        Label nameLabel = new Label
        {
            Text = modelOperator.OperatorName,
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
            Source = new ItemSelected<ContractOperatorModel>
            {
                SelectedItem = modelOperator,
                SelectedItemIndex = rowCount
            }
        };

        Binding command = new Binding("OperatorSelectedCommand", source: this.viewModel);

        TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, command);
        tapGesture.SetBinding(TapGestureRecognizer.CommandParameterProperty, commandParameter);

        tile.GestureRecognizers.Add(tapGesture);

        return tile;
    }
}
