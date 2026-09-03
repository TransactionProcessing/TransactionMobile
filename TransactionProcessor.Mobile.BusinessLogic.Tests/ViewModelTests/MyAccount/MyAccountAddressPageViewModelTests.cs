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
public class MyAccountAddressPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;
    private INavigationParameterServiceImposter NavigationParameterService;
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly IDialogServiceImposter DialogService;
    private readonly IMediatorImposter Mediator;
    private readonly MyAccountAddressPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public MyAccountAddressPageViewModelTests()
    {
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.Mediator = new IMediatorImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new MyAccountAddressPageViewModel(this.NavigationService.Instance(),
                                                           this.ApplicationCache.Instance(),
                                                           this.DialogService.Instance(),
                                                           this.DeviceService.Instance(),
                                                           this.Mediator.Instance(),
                                                           this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task MyAccountAddressPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
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

        this.NavigationService.GoBack().Called(Count.Once());
    }
}