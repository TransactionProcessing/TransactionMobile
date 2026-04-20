using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.MyAccount;

[Collection("ViewModelTests")]
public class MyAccountContactPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly MyAccountContactPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;
    private readonly Mock<IMediator> Mediator;

    public MyAccountContactPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.Mediator = new Mock<IMediator>();

        this.ViewModel = new MyAccountContactPageViewModel(this.NavigationService.Object,
                                                           this.ApplicationCache.Object,
                                                           this.DialogService.Object, this.DeviceService.Object,
                                                           this.NavigationParameterService.Object,
                                                           this.Mediator.Object);
    }

    [Fact]
    public async Task MyAccountContactPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantDetailsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.Mediator.Verify(m => m.Send(It.IsAny<MerchantQueries.GetMerchantDetailsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
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