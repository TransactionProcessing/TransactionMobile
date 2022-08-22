namespace TransactionMobile.Maui.Pages.MyAccount;

using BusinessLogic.Common;
using BusinessLogic.Models;
using BusinessLogic.ViewModels.MyAccount;

public partial class MyAccountPage : ContentPage
{
    private MyAccountPageViewModel viewModel => BindingContext as MyAccountPageViewModel;

    public MyAccountPage(MyAccountPageViewModel vm)
    {
        InitializeComponent();
        
        BindingContext = vm;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.Initialise(CancellationToken.None);
        this.LoadOptions(viewModel);
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
                AutomationId = modelOption.Title,
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