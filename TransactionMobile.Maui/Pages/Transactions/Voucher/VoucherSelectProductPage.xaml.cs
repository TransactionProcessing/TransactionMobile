namespace TransactionMobile.Maui;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class VoucherSelectProductPage : ContentPage
{
    private VoucherSelectProductPageViewModel viewModel => BindingContext as VoucherSelectProductPageViewModel;

	public VoucherSelectProductPage(VoucherSelectProductPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.Initialise(CancellationToken.None);
        this.LoadProducts(viewModel);
    }

    private void LoadProducts(VoucherSelectProductPageViewModel viewModel)
    {
        RowDefinitionCollection rowDefinitionCollection = new RowDefinitionCollection();
        for (Int32 i = 0; i < viewModel.Products.Count; i++)
        {
            rowDefinitionCollection.Add(new RowDefinition
                                        {
                                            Height = 60
                                        });
        }

        this.ProductsGrid.RowDefinitions = rowDefinitionCollection;

        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products)
        {
            Button button = new Button
            {
                                  Text = modelProduct.ProductDisplayText,
                                  HorizontalOptions = LayoutOptions.FillAndExpand,
                                  AutomationId = modelProduct.ProductDisplayText,
            };
            //button.SetDynamicResource(VisualElement.StyleProperty, "MobileTopupButtonStyle");

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

            this.ProductsGrid.Add(button, 0, rowCount);

            rowCount++;
        }
    }
}