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
            Button button = new Button
            {
                                  Text = modelProduct.ProductDisplayText,
                                  HorizontalOptions = LayoutOptions.FillAndExpand,
                                  AutomationId = modelProduct.ProductDisplayText,
            };
            button.SetDynamicResource(VisualElement.StyleProperty, "VoucherButtonStyle");

            Binding commandParameter = new Binding
            {
                Source = new ItemSelected<ContractProductModel>
                {
                    SelectedItem = modelProduct,
                    SelectedItemIndex = rowCount
                }
            };

            Binding command = new Binding("ProductSelectedCommand");

            // Create the behavior and bind it to the command
            EventToCommandBehavior behavior = new EventToCommandBehavior
            {
                EventName = "Clicked"
            };
            behavior.SetBinding(EventToCommandBehavior.CommandProperty, command);
            behavior.SetBinding(EventToCommandBehavior.CommandParameterProperty, commandParameter);

            // Attach the behavior to the button
            button.Behaviors.Add(behavior);

            this.ProductsList.Add(button);

            rowCount++;
        }
        this.ProductsList.Add(this.AddBackButton());
    }

    private Button AddBackButton()
    {
        Button button = new Button
                        {
                            Text = "Back",
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            AutomationId = "BackButton",
                        };
        button.SetDynamicResource(VisualElement.StyleProperty, "VoucherButtonStyle");

        Binding backButtonCommand = new Binding
                                    {
                                        Source = this.viewModel.BackButtonCommand
                                    };

        button.SetBinding(Button.CommandProperty, backButtonCommand);

        return button;
    }
}