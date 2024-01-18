using TransactionMobile.Maui.Pages.Common;

namespace TransactionMobile.Maui.Pages.Reports;

using BusinessLogic.Common;
using BusinessLogic.Models;
using BusinessLogic.ViewModels.Reports;

public partial class ReportsPage : NoBackWithoutLogoutPage
{
    private ReportsPageViewModel viewModel => BindingContext as ReportsPageViewModel;

    public ReportsPage(ReportsPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.LoadProducts(this.viewModel);
    }

    private void LoadProducts(ReportsPageViewModel viewModel)
    {
        this.ReportsList.Clear();

        Int32 rowCount = 0;
        foreach (ListViewItem modelOption in viewModel.ReportsMenuOptions)
        {
            Button button = new Button
                            {
                                Text = modelOption.Title,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                AutomationId = $"{modelOption.Title.Replace(" ", "")}Button",
                            };
            button.SetDynamicResource(VisualElement.StyleProperty, "ReportsButtonStyle");

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
        }

        this.ReportsList.Add(this.AddBackButton());
    }

    private Button AddBackButton()
    {
        Button button = new Button
        {
            Text = "Back",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            AutomationId = "BackButton",
        };
        button.SetDynamicResource(VisualElement.StyleProperty, "ReportsButtonStyle");

        Binding backButtonCommand = new Binding
        {
            Source = viewModel.BackButtonCommand
        };

        button.SetBinding(Button.CommandProperty, backButtonCommand);

        return button;
    }

}