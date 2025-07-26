using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

public class MyAccountContactPageViewModel : ExtendedBaseViewModel
{
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
                                         INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService) {
        this.Title = "My Contacts";
    }


    #endregion

    public async Task Initialise(CancellationToken none) {
        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();

        // TODO: handle a null
        this.Contact = merchantDetails.Contact;
    }
}