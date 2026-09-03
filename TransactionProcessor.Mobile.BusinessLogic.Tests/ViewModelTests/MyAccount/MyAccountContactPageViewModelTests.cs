using MediatR;
using Imposter.Abstractions;
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
    private readonly INavigationServiceImposter NavigationService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogService;

    private readonly MyAccountContactPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;
    private readonly IMediatorImposter Mediator;

    public MyAccountContactPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.Mediator = new IMediatorImposter();

        this.ViewModel = new MyAccountContactPageViewModel(this.NavigationService.Instance(),
                                                           this.ApplicationCache.Instance(),
                                                           this.DialogService.Instance(), this.DeviceService.Instance(),
                                                           this.NavigationParameterService.Instance(),
                                                           this.Mediator.Instance());
    }

    [Fact]
    public async Task MyAccountContactPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.ViewModel.Contact.ShouldNotBeNull();
        this.ViewModel.Contact.EmailAddress.ShouldBe(TestData.ContactEmailAddress);
        this.ViewModel.Contact.Name.ShouldBe(TestData.ContactName);
        this.ViewModel.Contact.MobileNumber.ShouldBe(TestData.ContactMobileNumber);
    }

    [Fact]
    public async Task MyAccountContactPageViewModel_BackButtonCommand_PreviousPageIsShown()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }
}