using CommunityToolkit.Maui.Behaviors;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;

public partial class MobileTopupSelectOperatorPage : ContentPage
{
    private MobileTopupSelectOperatorPageViewModel viewModel => this.BindingContext as MobileTopupSelectOperatorPageViewModel;

public MobileTopupSelectOperatorPage(MobileTopupSelectOperatorPageViewModel vm)
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

    private void LoadOperators(MobileTopupSelectOperatorPageViewModel viewModel)
    {
        this.OperatorList.Children.Clear();

        Int32 rowCount = 0;
        foreach (ContractOperatorModel modelOperator in viewModel.Operators)
        {
            Frame tile = this.CreateTile(modelOperator.OperatorName, (Color)Application.Current.Resources["mobileTopup"], modelOperator.OperatorName);
            TapGestureRecognizer tap = new TapGestureRecognizer
            {
                Command = new Command(() => viewModel.OperatorSelectedCommand.Execute(
                    new ItemSelected<ContractOperatorModel> { SelectedItem = modelOperator, SelectedItemIndex = rowCount }))
            };
            tile.GestureRecognizers.Add(tap);
            this.OperatorList.Children.Add(tile);
            rowCount++;
        }

        this.OperatorList.Children.Add(this.CreateBackTile(viewModel.BackButtonCommand));
    }

    private Frame CreateTile(string text, Color backgroundColor, string automationId)
    {
        Frame frame = new Frame
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
        return frame;
    }

    private Frame CreateBackTile(System.Windows.Input.ICommand command)
    {
        Frame frame = this.CreateTile("Back", (Color)Application.Current.Resources["MidGray"], "BackButton");
        frame.GestureRecognizers.Add(new TapGestureRecognizer { Command = command });
        return frame;
    }
}
