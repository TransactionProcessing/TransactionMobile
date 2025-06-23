using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.MyAccount;

public partial class MyAccountPage : NoBackWithoutLogoutPage
{
    private MyAccountPageViewModel viewModel => BindingContext as MyAccountPageViewModel;

    public MyAccountPage(MyAccountPageViewModel vm)
    {
        this.InitializeComponent();
        
        BindingContext = vm;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadOptions(this.viewModel);
    }

    private void LoadOptions(MyAccountPageViewModel viewModel)
    {
        this.MyAccountOptionsList.Clear();

        Int32 rowCount = 0;
        foreach (ListViewItem modelOption in viewModel.MyAccountOptions)
        {
            Button button = new Button
            {
                Text = modelOption.Title,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                AutomationId = $"{modelOption.Title.Replace(" ", "")}Button",
            };
            button.SetDynamicResource(VisualElement.StyleProperty, "MyAccountButtonStyle");

            Binding commandParameter = new Binding
            {
                Source = new ItemSelected<ListViewItem>
                {
                    SelectedItem = modelOption,
                    SelectedItemIndex = rowCount
                }
            };

            Binding command = new Binding
            {
                Source = viewModel.OptionSelectedCommand
            };

            button.SetBinding(Button.CommandProperty, command);
            button.SetBinding(Button.CommandParameterProperty, commandParameter);

            this.MyAccountOptionsList.Add(button);

            rowCount++;
        }
    }

}