namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.MyAccount;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Moq;
using Requests;
using Services;
using Shared.Logger;
using Shouldly;
using ViewModels.MyAccount;
using Xunit;

public class MyAccountPageViewModelTests
{
    #region Methods

    [Fact]
    public async Task MyAccountPageViewModel_Initialise_IsInitialised() {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetMerchantDetailsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantDetailsModel);

        MyAccountPageViewModel viewModel = new MyAccountPageViewModel(navigationService.Object, applicationCache.Object, mediator.Object);
        await viewModel.Initialise(CancellationToken.None);

        viewModel.MerchantName.ShouldBe(TestData.MerchantDetailsModel.MerchantName);
        viewModel.LastLogin.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(30));
        applicationCache.Verify(a => a.SetMerchantDetails(It.IsAny<MerchantDetailsModel>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_AccountInfo_Execute_IsExecuted() {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IMediator> mediator = new Mock<IMediator>();
        MyAccountPageViewModel viewModel = new MyAccountPageViewModel(navigationService.Object, applicationCache.Object, mediator.Object);
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.AccountInfo));

        navigationService.Verify(n => n.GoToMyAccountDetails(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Addresses_Execute_IsExecuted() {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IMediator> mediator = new Mock<IMediator>();

        MyAccountPageViewModel viewModel = new MyAccountPageViewModel(navigationService.Object, applicationCache.Object, mediator.Object);
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Addresses));

        navigationService.Verify(n => n.GoToMyAccountAddresses(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Contacts_Execute_IsExecuted() {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IMediator> mediator = new Mock<IMediator>();

        MyAccountPageViewModel viewModel = new MyAccountPageViewModel(navigationService.Object, applicationCache.Object, mediator.Object);
        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Contacts));

        navigationService.Verify(n => n.GoToMyAccountContacts(), Times.Once);
    }

    [Fact]
    public void MyAccountPageViewModel_OptionSelectedCommand_Logout_Execute_IsExecuted() {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IMediator> mediator = new Mock<IMediator>();
        MyAccountPageViewModel viewModel = new MyAccountPageViewModel(navigationService.Object, applicationCache.Object, mediator.Object);

        viewModel.OptionSelectedCommand.Execute(this.CreateItemSelected(MyAccountPageViewModel.AccountOptions.Logout));

        navigationService.Verify(n => n.GoToLoginPage(), Times.Once);
    }

    private ItemSelected<ListViewItem> CreateItemSelected(MyAccountPageViewModel.AccountOptions selectedOption) {
        ItemSelected<ListViewItem> i = new ItemSelected<ListViewItem>();
        i.SelectedItemIndex = (Int32)selectedOption;
        return i;
    }

    #endregion
}