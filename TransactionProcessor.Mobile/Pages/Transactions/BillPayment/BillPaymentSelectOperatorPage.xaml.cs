using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentSelectOperatorPage : ContentPage
{
    private BillPaymentSelectOperatorPageViewModel viewModel => this.BindingContext as BillPaymentSelectOperatorPageViewModel;

	public BillPaymentSelectOperatorPage(BillPaymentSelectOperatorPageViewModel vm)
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

    private void LoadOperators(BillPaymentSelectOperatorPageViewModel viewModel)
    {
        this.OperatorList.Children.Clear();
        
        Int32 rowCount = 0;
        foreach (ContractOperatorModel modelOperator in viewModel.Operators)
        {
            Button button = new Button
                            {
                                Text = modelOperator.OperatorName,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                AutomationId = modelOperator.OperatorName,
                            };
            button.SetDynamicResource(VisualElement.StyleProperty, "BillPaymentButtonStyle");
            
            Binding commandParameter = new Binding { Source = new ItemSelected<ContractOperatorModel>() { SelectedItem = modelOperator, SelectedItemIndex = rowCount } };

            Binding command = new Binding("OperatorSelectedCommand", source: this.viewModel);

            // Create the behavior and bind it to the command
            EventToCommandBehavior behavior = new EventToCommandBehavior
            {
                EventName = "Clicked"
            };
            behavior.SetBinding(EventToCommandBehavior.CommandProperty, command);
            behavior.SetBinding(EventToCommandBehavior.CommandParameterProperty, commandParameter);

            // Attach the behavior to the button
            button.Behaviors.Add(behavior);

            this.OperatorList.Add(button);

            rowCount++;
        }
        this.OperatorList.Add(this.AddBackButton());
        
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