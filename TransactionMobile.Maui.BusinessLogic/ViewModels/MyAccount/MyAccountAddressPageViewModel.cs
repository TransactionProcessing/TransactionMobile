namespace TransactionMobile.Maui.BusinessLogic.ViewModels.MyAccount;

using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers;
using Services;

public class MyAccountAddressPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    private readonly IApplicationCache ApplicationCache;

    private readonly IMediator Mediator;

    #region Constructors

    public MyAccountAddressPageViewModel(INavigationService navigationService,
                                         IApplicationCache applicationCache,
                                         IMediator mediator) {
        this.NavigationService = navigationService;
        this.ApplicationCache = applicationCache;
        this.Mediator = mediator;
        this.Title = "My Addresses";
    }
    
    #endregion

    private AddressModel address;

    public AddressModel Address{
        get => this.address;
        set => this.SetProperty(ref this.address, value);
    }

    #region Properties

    public async Task Initialise(CancellationToken cancellationToken) {
        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();

        // TODO: handle a null

        this.Address = merchantDetails.Address;
    }

    #endregion
}