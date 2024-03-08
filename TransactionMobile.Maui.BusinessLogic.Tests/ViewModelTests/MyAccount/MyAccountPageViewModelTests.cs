namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.MyAccount;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Common;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using SimpleResults;
using UIServices;
using ViewModels;
using ViewModels.MyAccount;
using Xunit;

[Collection("ViewModelTests")]
public class MyAccountPageViewModelTests
{
    #region Methods

    private Mock<INavigationService> navigationService;

    private Mock<IApplicationCache> applicationCache;

    private Mock<IDialogService> dialogService;

    private Mock<IMediator> mediator;

    private MyAccountPageViewModel viewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public MyAccountPageViewModelTests() {
        navigationService = new Mock<INavigationService>();
        applicationCache = new Mock<IApplicationCache>();
        dialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        mediator = new Mock<IMediator>();
        viewModel = new MyAccountPageViewModel(navigationService.Object, applicationCache.Object,
                                                                      dialogService.Object, this.DeviceService.Object,
                                                                      mediator.Object);
    }

    [Fact]
    public async Task MyAccountPageViewModel_Initialise_IsInitialised() {
        
        mediator.Setup(m => m.Send(It.IsAny<GetMerchantDetailsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success( TestData.MerchantDetailsModel));

        await viewModel.Initialise(CancellationToken.None);

        viewModel.MerchantName.ShouldBe(TestData.MerchantDetailsModel.MerchantName);
        viewModel.LastLogin.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(30));
        applicationCache.Verify(a => a.SetMerchantDetails(It.IsAny<MerchantDetailsModel>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_AccountInfo_Execute_IsExecuted() {
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.AccountInfo));

        navigationService.Verify(n => n.GoToMyAccountDetails(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Addresses_Execute_IsExecuted() {
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Addresses));

        navigationService.Verify(n => n.GoToMyAccountAddresses(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Contacts_Execute_IsExecuted() {
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Contacts));

        navigationService.Verify(n => n.GoToMyAccountContacts(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Logout_Execute_IsExecuted() {
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Logout));

        navigationService.Verify(n => n.GoToLoginPage(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Unsupported_Execute_IsExecuted()
    {
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected((MyAccountPageViewModel.AccountOptions)99));

        navigationService.Verify(n => n.GoToMyAccountDetails(), Times.Never);
    }

    [Fact]
    public void MyAccountPageViewModel_GetMyAccountOptions_OptionsReturned(){
        viewModel.MyAccountOptions = new List<ListViewItem>{
                                                               new ListViewItem{
                                                                                   Title = "Test"
                                                                               }
                                                           };
        List<ListViewItem> options = viewModel.MyAccountOptions;
        options.Count.ShouldBe(1);
    }

    [Fact]
    public async Task MyAccountPageViewModel_BackButtonCommand_HomePageIsShown()
    {
        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n=> n.GoToHome(),Times.Once);
    }

    private ItemSelected<ListViewItem> CreateItemSelected(MyAccountPageViewModel.AccountOptions selectedOption) {
        ItemSelected<ListViewItem> i = new ItemSelected<ListViewItem>();
        i.SelectedItemIndex = (Int32)selectedOption;
        return i;
    }

    #endregion
}