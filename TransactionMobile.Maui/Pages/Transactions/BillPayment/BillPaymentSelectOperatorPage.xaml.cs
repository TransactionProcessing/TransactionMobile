using System;
using System.Threading;
using Microsoft.Maui.Controls;

namespace TransactionMobile.Maui.Pages.Transactions.BillPayment;

using BusinessLogic.Common;
using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentSelectOperatorPage : ContentPage
{
    private BillPaymentSelectOperatorPageViewModel viewModel => BindingContext as BillPaymentSelectOperatorPageViewModel;

	public BillPaymentSelectOperatorPage(BillPaymentSelectOperatorPageViewModel vm)
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
                                        Source = viewModel.BackButtonCommand
                                    };

        button.SetBinding(Button.CommandProperty, backButtonCommand);

        return button;
    }
}