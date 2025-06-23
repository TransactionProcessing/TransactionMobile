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

        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products)
        {
            Button button = new Button
            {
                                  Text = modelProduct.ProductDisplayText,
                                  HorizontalOptions = LayoutOptions.FillAndExpand,
                                  AutomationId = modelProduct.ProductDisplayText,
            };
            button.SetDynamicResource(VisualElement.StyleProperty, "BillPaymentButtonStyle");

            Binding commandParameter = new Binding()
                                       {
                                           Source = new ItemSelected<ContractProductModel>
                                                    {
                                                        SelectedItem = modelProduct,
                                                        SelectedItemIndex = rowCount
                                                    }
                                       };

            Binding command = new Binding
                              {
                                  Source = viewModel.ProductSelectedCommand
                              };

            button.SetBinding(Button.CommandProperty, command);
            button.SetBinding(Button.CommandParameterProperty, commandParameter);

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
        button.SetDynamicResource(VisualElement.StyleProperty, "BillPaymentButtonStyle");

        Binding backButtonCommand = new Binding
                                    {
                                        Source = this.viewModel.BackButtonCommand
                                    };

        button.SetBinding(Button.CommandProperty, backButtonCommand);

        return button;
    }
}