namespace TransactionMobile.Maui;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class VoucherSelectOperatorPage : ContentPage
{
    private VoucherSelectOperatorPageViewModel viewModel => BindingContext as VoucherSelectOperatorPageViewModel;

	public VoucherSelectOperatorPage(VoucherSelectOperatorPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.Initialise(CancellationToken.None);
        this.LoadOperators(viewModel);
    }

    private void LoadOperators(VoucherSelectOperatorPageViewModel viewModel)
    {
        RowDefinitionCollection rowDefinitionCollection = new RowDefinitionCollection();
        for (Int32 i = 0; i < viewModel.Operators.Count; i++)
        {
            rowDefinitionCollection.Add(new RowDefinition
                                        {
                                            Height = 60
                                        });
        }

        this.OperatorGrid.RowDefinitions = rowDefinitionCollection;

        Int32 rowCount = 0;
        foreach (ContractOperatorModel modelOperator in viewModel.Operators)
        {
            Button button = new Button
                            {
                                Text = modelOperator.OperatorName,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                AutomationId = modelOperator.OperatorName,
                            };
            //button.SetDynamicResource(VisualElement.StyleProperty, "MobileTopupButtonStyle");
            Binding commandParameter = new Binding()
                                       {
                                           Source = new ItemSelected<ContractOperatorModel>(){ 
                                                                            SelectedItem = modelOperator,
                                                                            SelectedItemIndex = rowCount
                                                                        }
                                       };

            Binding command = new Binding
                        {
                            Source = viewModel.OperatorSelectedCommand
                        };
            
            button.SetBinding(Button.CommandProperty, command);
            button.SetBinding(Button.CommandParameterProperty, commandParameter);

            this.OperatorGrid.Add(button, 0, rowCount);

            rowCount++;
        }

        
    }
}