namespace TransactionMobile.Maui;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupSelectProductPage : ContentPage
{
    private MobileTopupSelectProductPageViewModel viewModel => BindingContext as MobileTopupSelectProductPageViewModel;

	public MobileTopupSelectProductPage(MobileTopupSelectProductPageViewModel vm)
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

    private void LoadProducts(MobileTopupSelectProductPageViewModel viewModel)
    {
        this.ProductsList.Clear();
        
        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products)
        {
            Button button = new Button
            {
                                  Text = modelProduct.ProductDisplayText,
                                  HorizontalOptions = LayoutOptions.FillAndExpand,
                                  AutomationId = modelProduct.ProductDisplayText,
            };
            button.SetDynamicResource(VisualElement.StyleProperty, "MobileTopupButtonStyle");

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
    }
}