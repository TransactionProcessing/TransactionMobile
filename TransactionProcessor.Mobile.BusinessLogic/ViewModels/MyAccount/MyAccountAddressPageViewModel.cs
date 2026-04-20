using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

public class MyAccountAddressPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private AddressModel address;

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public MyAccountAddressPageViewModel(INavigationService navigationService,
                                         IApplicationCache applicationCache,
                                         IDialogService dialogService,
                                         IDeviceService deviceService,
                                         IMediator mediator, INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService,navigationParameterService) {
        this.Mediator = mediator;
        this.Title = "My Addresses";
    }

    #endregion

    #region Properties

    public AddressModel Address {
        get => this.address;
        set => this.SetProperty(ref this.address, value);
    }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken) {
        MerchantQueries.GetMerchantDetailsQuery query = new MerchantQueries.GetMerchantDetailsQuery();

        Result<MerchantDetailsModel> merchantDetailsResult = await this.Mediator.Send(query, cancellationToken);
        if (merchantDetailsResult.IsFailed) {
            await this.DialogService.ShowWarningToast("Unable to load merchant details. Please try again later.", cancellationToken: cancellationToken);
            return;
        }

        this.Address = merchantDetailsResult.Data.Address;
    }

    #endregion
}