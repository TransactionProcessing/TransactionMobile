using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class ReportsPage : NoBackWithoutLogoutPage
{
    private ReportsPageViewModel viewModel => BindingContext as ReportsPageViewModel;

    public ReportsPage(ReportsPageViewModel vm)
	{
		this.InitializeComponent();
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

            Binding command = new Binding("OptionSelectedCommand");

            // Create the behavior and bind it to the command
            EventToCommandBehavior behavior = new EventToCommandBehavior
            {
                EventName = "Clicked"
            };
            behavior.SetBinding(EventToCommandBehavior.CommandProperty, command);
            behavior.SetBinding(EventToCommandBehavior.CommandParameterProperty, commandParameter);

            // Attach the behavior to the button
            button.Behaviors.Add(behavior);

            rowCount++;
            this.ReportsList.Add(button);

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
            Source = this.viewModel.BackButtonCommand
        };

        button.SetBinding(Button.CommandProperty, backButtonCommand);

        return button;
    }

}