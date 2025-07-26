using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;

public partial class MobileTopupSelectProductPage : ContentPage
{
    #region Constructors

    public MobileTopupSelectProductPage(MobileTopupSelectProductPageViewModel vm) {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    #endregion

    #region Properties

    private MobileTopupSelectProductPageViewModel viewModel => this.BindingContext as MobileTopupSelectProductPageViewModel;

    #endregion

    #region Methods

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadProducts(this.viewModel);
    }

    private void LoadProducts(MobileTopupSelectProductPageViewModel viewModel) {
        this.ProductsList.Clear();

        Int32 rowCount = 0;
        foreach (ContractProductModel modelProduct in viewModel.Products) {
            Button button = new Button {
                                           Text = modelProduct.ProductDisplayText,
                                           HorizontalOptions = LayoutOptions.FillAndExpand,
                                           AutomationId = modelProduct.ProductDisplayText,
                                       };
            button.SetDynamicResource(VisualElement.StyleProperty, "MobileTopupButtonStyle");

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
        button.SetDynamicResource(VisualElement.StyleProperty, "MobileTopupButtonStyle");

        Binding backButtonCommand = new Binding
                                    {
                                        Source = this.viewModel.BackButtonCommand
                                    };

        button.SetBinding(Button.CommandProperty, backButtonCommand);

        return button;
    }

    #endregion
}