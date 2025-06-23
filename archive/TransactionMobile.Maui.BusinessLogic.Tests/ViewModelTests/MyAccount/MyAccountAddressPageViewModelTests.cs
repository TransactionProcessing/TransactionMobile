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
public class MyAccountAddressPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private Mock<INavigationParameterService> NavigationParameterService;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogService;
    private readonly Mock<IMediator> Mediator;
    private readonly MyAccountAddressPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public MyAccountAddressPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.Mediator = new Mock<IMediator>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new MyAccountAddressPageViewModel(this.NavigationService.Object,
                                                           this.ApplicationCache.Object,
                                                           this.DialogService.Object,
                                                           this.DeviceService.Object,
                                                           this.Mediator.Object,
                                                           this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task MyAccountAddressPageViewModel_Initialise_IsInitialised()
    {
        this.ApplicationCache.Setup(a => a.GetMerchantDetails()).Returns(TestData.MerchantDetailsModel);

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ApplicationCache.Verify(a => a.GetMerchantDetails(), Times.Once);
        this.ViewModel.Address.ShouldNotBeNull();
        this.ViewModel.Address.AddressLine1.ShouldBe(TestData.AddressLine1);
        this.ViewModel.Address.AddressLine2.ShouldBe(TestData.AddressLine2);
        this.ViewModel.Address.AddressLine3.ShouldBe(TestData.AddressLine3);
        this.ViewModel.Address.AddressLine4.ShouldBe(TestData.AddressLine4);
        this.ViewModel.Address.Region.ShouldBe(TestData.Region);
        this.ViewModel.Address.Town.ShouldBe(TestData.Town);
        this.ViewModel.Address.PostalCode.ShouldBe(TestData.PostalCode);
    }

    [Fact]
    public async Task MyAccountAddressPageViewModel_BackButtonCommand_PreviousPageIsShown()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}