using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

public class MyAccountContactPageViewModel : ExtendedBaseViewModel
{
    private readonly IMediator Mediator;
    private ContactModel contact;

    public ContactModel Contact {
        get => this.contact;
        set => this.SetProperty(ref this.contact, value);
    }

    #region Constructors

    public MyAccountContactPageViewModel(INavigationService navigationService,
                                         IApplicationCache applicationCache,
                                         IDialogService dialogService,
                                         IDeviceService deviceService,
                                         INavigationParameterService navigationParameterService,
                                         IMediator mediator) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService) {
        this.Mediator = mediator;
        this.Title = "My Contacts";
    }


    #endregion

    public async Task Initialise(CancellationToken cancellationToken) {
        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();

        if (merchantDetails == null)
        {
            MerchantQueries.GetMerchantDetailsQuery query = new MerchantQueries.GetMerchantDetailsQuery();

            Result<MerchantDetailsModel> merchantDetailsResult = await this.Mediator.Send(query, cancellationToken);
            if (merchantDetailsResult.IsFailed)
            {
                await this.DialogService.ShowWarningToast("Unable to load merchant details. Please try again later.", cancellationToken: cancellationToken);
                return;
            }

            DateTime expirationTime = DateTime.Now.AddMinutes(60);
            CancellationChangeToken expirationToken = new(new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token);
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                // Pin to cache.
                .SetPriority(CacheItemPriority.NeverRemove)
                // Set the actual expiration time
                .SetAbsoluteExpiration(expirationTime)
                // Force eviction to run
                .AddExpirationToken(expirationToken);

            this.ApplicationCache.SetMerchantDetails(merchantDetailsResult.Data, cacheEntryOptions);
            merchantDetails = this.ApplicationCache.GetMerchantDetails();
        }

        this.Contact = merchantDetails.Contact;
    }
}