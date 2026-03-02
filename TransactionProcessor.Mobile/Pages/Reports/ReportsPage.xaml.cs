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
            Frame tile = this.CreateTile(modelOption.Title, (Color)Application.Current.Resources["reports"], $"{modelOption.Title.Replace(" ", "")}Button");
            TapGestureRecognizer tap = new TapGestureRecognizer
            {
                Command = new Command(() => viewModel.OptionSelectedCommand.Execute(
                    new ItemSelected<ListViewItem> { SelectedItem = modelOption, SelectedItemIndex = rowCount }))
            };
            tile.GestureRecognizers.Add(tap);
            this.ReportsList.Add(tile);
            rowCount++;
        }

        this.ReportsList.Add(this.CreateBackTile(viewModel.BackButtonCommand));
    }

    private Frame CreateTile(string text, Color backgroundColor, string automationId)
    {
        return new Frame
        {
            CornerRadius = 14,
            HasShadow = true,
            Padding = new Thickness(14),
            HeightRequest = 56,
            BackgroundColor = backgroundColor,
            BorderColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            AutomationId = automationId,
            Content = new Label
            {
                Text = text,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            }
        };
    }

    private Frame CreateBackTile(System.Windows.Input.ICommand command)
    {
        Frame frame = this.CreateTile("Back", (Color)Application.Current.Resources["MidGray"], "BackButton");
        frame.GestureRecognizers.Add(new TapGestureRecognizer { Command = command });
        return frame;
    }
}
