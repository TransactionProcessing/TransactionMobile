using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
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

    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly Mock<IMediator> Mediator;

    private readonly MyAccountPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public MyAccountPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.Mediator = new Mock<IMediator>();
        this.ViewModel = new MyAccountPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object,
                                                                      this.DialogService.Object, this.DeviceService.Object,
                                                                      this.Mediator.Object,
                                                                      this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task MyAccountPageViewModel_Initialise_IsInitialised() {
        
        this.Mediator.Setup(m => m.Send(It.IsAny<GetMerchantDetailsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success( TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.MerchantName.ShouldBe(TestData.MerchantDetailsModel.MerchantName);
        this.ViewModel.LastLogin.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(30));
        this.ApplicationCache.Verify(a => a.SetMerchantDetails(It.IsAny<MerchantDetailsModel>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_AccountInfo_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.AccountInfo));

        this.NavigationService.Verify(n => n.GoToMyAccountDetails(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Addresses_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Addresses));

        this.NavigationService.Verify(n => n.GoToMyAccountAddresses(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Contacts_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Contacts));

        this.NavigationService.Verify(n => n.GoToMyAccountContacts(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Logout_Execute_IsExecuted() {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Logout));

        this.NavigationService.Verify(n => n.GoToLoginPage(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Unsupported_Execute_IsExecuted()
    {
        this.ViewModel.OptionSelectedCommand.Execute(this.CreateItemSelected((MyAccountPageViewModel.AccountOptions)99));

        this.NavigationService.Verify(n => n.GoToMyAccountDetails(), Times.Never);
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

        this.NavigationService.Verify(n=> n.GoToHome(),Times.Once);
    }

    private ItemSelected<ListViewItem> CreateItemSelected(MyAccountPageViewModel.AccountOptions selectedOption) {
        ItemSelected<ListViewItem> i = new ItemSelected<ListViewItem>();
        i.SelectedItemIndex = (Int32)selectedOption;
        return i;
    }

    #endregion
}