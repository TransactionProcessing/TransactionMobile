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
        this.MyAccountOptionsList.Children.Clear();

        Int32 rowCount = 0;
        foreach (ListViewItem modelOption in viewModel.MyAccountOptions)
        {
            Frame tile = this.CreateOptionTile(modelOption, rowCount);
            this.MyAccountOptionsList.Children.Add(tile);
            rowCount++;
        }
    }

    private Frame CreateOptionTile(ListViewItem modelOption, Int32 rowCount)
    {
        Frame tile = new Frame();
        tile.SetDynamicResource(VisualElement.StyleProperty, "SelectionTileFrame");
        tile.AutomationId = $"{modelOption.Title.Replace(" ", "")}Button";

        String iconSource = modelOption.Title switch
        {
            "Addresses" => "homebutton.svg",
            "Contacts" => "supportbutton.svg",
            "Account Info" => "reportbutton.svg",
            "Logout" => "backbutton.svg",
            _ => "profilebutton.svg"
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
        nameLabel.SetDynamicResource(Label.TextColorProperty, "profile");

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

    private async void DarkThemeSwitch_Toggled(Object sender,
                                               ToggledEventArgs e)
    {
        if (this.viewModel == null)
        {
            return;
        }

        await this.viewModel.SetDarkTheme(e.Value);
        this.LoadOptions(this.viewModel);
    }
}
