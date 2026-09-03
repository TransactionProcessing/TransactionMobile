using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.MyAccount;

[Collection("ViewModelTests")]
public class MyAccountPageViewModelTests
{
    #region Methods

    private readonly INavigationServiceImposter NavigationService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogService;

    private readonly IMediatorImposter Mediator;
    private readonly IApplicationThemeServiceImposter ApplicationThemeService;

    private readonly MyAccountPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public MyAccountPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.Mediator = new IMediatorImposter();
        this.ApplicationThemeService = new IApplicationThemeServiceImposter();
        this.ViewModel = new MyAccountPageViewModel(this.NavigationService.Instance(), this.ApplicationCache.Instance(),
                                                                       this.DialogService.Instance(), this.DeviceService.Instance(),
                                                                       this.ApplicationThemeService.Instance(),
                                                                       this.Mediator.Instance(),
                                                                       this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task MyAccountPageViewModel_Initialise_IsInitialised() {
        this.ApplicationThemeService.GetDarkThemeEnabled().ReturnsAsync(true);
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success( TestData.MerchantDetailsModel));
        this.ApplicationCache.GetLastLoginDate().Returns(DateTime.Now);
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.MerchantName.ShouldBe(TestData.MerchantDetailsModel.MerchantName);
        this.ViewModel.LastLogin.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(30));
        this.ViewModel.IsDarkThemeEnabled.ShouldBeTrue();
    }

    [Fact]
    public async Task MyAccountPageViewModel_Initialise_WhenMerchantDetailsAreCached_UsesCachedMerchantDetails() {
        this.ApplicationCache.GetMerchantDetails().Returns(TestData.MerchantDetailsModel);
        this.ApplicationCache.GetLastLoginDate().Returns(DateTime.Now);
        this.ApplicationThemeService.GetDarkThemeEnabled().ReturnsAsync(false);
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.MerchantName.ShouldBe(TestData.MerchantDetailsModel.MerchantName);
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task MyAccountPageViewModel_SetDarkTheme_ThemeStoredAndPropertyUpdated() {
        await this.ViewModel.SetDarkTheme(true);

        this.ViewModel.IsDarkThemeEnabled.ShouldBeTrue();
        this.ApplicationThemeService.SetDarkTheme(true).Called(Count.Once());
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_AccountInfo_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.AccountInfo));

        this.NavigationService.GoToMyAccountDetails().Called(Count.Once());
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Addresses_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Addresses));

        this.NavigationService.GoToMyAccountAddresses().Called(Count.Once());
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Contacts_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Contacts));

        this.NavigationService.GoToMyAccountContacts().Called(Count.Once());
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Logout_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Logout));

        this.NavigationService.GoToLoginPage().Called(Count.Once());
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Unsupported_Execute_IsExecuted()
    {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected((MyAccountPageViewModel.AccountOptions)99));

        this.NavigationService.GoToMyAccountDetails().Called(Count.Never());
    }

    [Fact]
    public void MyAccountPageViewModel_GetMyAccountOptions_OptionsReturned(){
        this.ViewModel.MyAccountOptions = new List<ListViewItem>{
                                                               new ListViewItem{
                                                                                   Title = "Test"
                                                                               }
                                                           };
        List<ListViewItem> options = this.ViewModel.MyAccountOptions;
        options.Count.ShouldBe(1);
    }

    [Fact]
    public async Task MyAccountPageViewModel_BackButtonCommand_HomePageIsShown()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoToHome().Called(Count.Once());
    }

    private ItemSelected<ListViewItem> CreateItemSelected(MyAccountPageViewModel.AccountOptions selectedOption) {
        ItemSelected<ListViewItem> i = new ItemSelected<ListViewItem>();
        i.SelectedItemIndex = (Int32)selectedOption;
        return i;
    }

    #endregion
}
