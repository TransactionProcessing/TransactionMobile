namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.MyAccount;

using System;
using System.Threading;
using System.Threading.Tasks;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using Shouldly;
using UIServices;
using ViewModels;
using ViewModels.MyAccount;
using Xunit;

[Collection("ViewModelTests")]
public class MyAccountContactPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly MyAccountContactPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public MyAccountContactPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new MyAccountContactPageViewModel(this.NavigationService.Object,
                                                           this.ApplicationCache.Object,
                                                           this.DialogService.Object, this.DeviceService.Object,
                                                           this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task MyAccountContactPageViewModel_Initialise_IsInitialised()
    {
        this.ApplicationCache.Setup(a => a.GetMerchantDetails()).Returns(TestData.MerchantDetailsModel);

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ApplicationCache.Verify(a => a.GetMerchantDetails(), Times.Once);
        this.ViewModel.Contact.ShouldNotBeNull();
        this.ViewModel.Contact.EmailAddress.ShouldBe(TestData.ContactEmailAddress);
        this.ViewModel.Contact.Name.ShouldBe(TestData.ContactName);
        this.ViewModel.Contact.MobileNumber.ShouldBe(TestData.ContactMobileNumber);
    }

    [Fact]
    public async Task MyAccountContactPageViewModel_BackButtonCommand_PreviousPageIsShown()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}