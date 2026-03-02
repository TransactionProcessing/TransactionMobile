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
        this.ReportsList.Children.Clear();
        this.ReportsList.RowDefinitions.Clear();

        var tiles = new List<Frame>();
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
            tiles.Add(tile);
            rowCount++;
        }

        int row = 0;
        for (int i = 0; i < tiles.Count; i += 2)
        {
            this.ReportsList.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(tiles[i], row);
            Grid.SetColumn(tiles[i], 0);
            this.ReportsList.Children.Add(tiles[i]);
            if (i + 1 < tiles.Count)
            {
                Grid.SetRow(tiles[i + 1], row);
                Grid.SetColumn(tiles[i + 1], 1);
                this.ReportsList.Children.Add(tiles[i + 1]);
            }
            row++;
        }

        Frame backTile = this.CreateBackTile(viewModel.BackButtonCommand);
        this.ReportsList.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        Grid.SetRow(backTile, row);
        Grid.SetColumn(backTile, 0);
        Grid.SetColumnSpan(backTile, 2);
        this.ReportsList.Children.Add(backTile);
    }

    private Frame CreateTile(string text, Color backgroundColor, string automationId)
    {
        return new Frame
        {
            CornerRadius = 16,
            HasShadow = true,
            Padding = new Thickness(12),
            HeightRequest = 100,
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
