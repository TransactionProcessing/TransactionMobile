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
        this.LoadReports(this.viewModel);
    }

    private void LoadReports(ReportsPageViewModel viewModel)
    {
        this.ReportsList.Children.Clear();

        Int32 rowCount = 0;
        foreach (ListViewItem modelOption in viewModel.ReportsMenuOptions)
        {
            Frame tile = this.CreateReportTile(modelOption, rowCount);
            this.ReportsList.Children.Add(tile);
            rowCount++;
        }
    }

    private Frame CreateReportTile(ListViewItem modelOption, Int32 rowCount)
    {
        Frame tile = new Frame();
        tile.SetDynamicResource(VisualElement.StyleProperty, "SelectionTileFrame");
        tile.AutomationId = $"{modelOption.Title.Replace(" ", "")}Button";

        String iconSource = modelOption.Title switch
        {
            "Sales Analysis" => "transactionsbutton.svg",
            "Balance Analysis" => "reportbutton.svg",
            _ => "reportbutton.svg"
        };

        Image icon = new Image
        {
            Source = ThemeButtonImageSource.Get(iconSource),
            HeightRequest = 36,
            WidthRequest = 36,
            HorizontalOptions = LayoutOptions.Center
        };

        Label nameLabel = new Label
        {
            Text = modelOption.Title,
            HorizontalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 13
        };
        nameLabel.SetDynamicResource(Label.TextColorProperty, "reports");

        tile.Content = new VerticalStackLayout
        {
            Spacing = 8,
            HorizontalOptions = LayoutOptions.Center,
            Children = { icon, nameLabel }
        };

        Binding commandParameter = new Binding
        {
            Source = new ItemSelected<ListViewItem>
            {
                SelectedItem = modelOption,
                SelectedItemIndex = rowCount
            }
        };

        Binding command = new Binding("OptionSelectedCommand", source: this.viewModel);

        TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, command);
        tapGesture.SetBinding(TapGestureRecognizer.CommandParameterProperty, commandParameter);

        tile.GestureRecognizers.Add(tapGesture);

        return tile;
    }
}
