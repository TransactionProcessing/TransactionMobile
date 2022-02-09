namespace TransactionMobile.Maui;

using BusinessLogic.Models;
using ViewModels.Transactions;

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
                                           Source = new SelectedItemChangedEventArgs(modelProduct, rowCount)
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