namespace TransactionMobile.Maui.BusinessLogic.ViewModels.MyAccount;

using Logging;
using Maui.UIServices;
using MediatR;
using MvvmHelpers;
using Services;
using TransactionMobile.Maui.BusinessLogic.Models;
using TransactionMobile.Maui.BusinessLogic.UIServices;

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
                                         IDialogService dialogService) : base(applicationCache, dialogService, navigationService) {
        this.Title = "My Contacts";
    }


    #endregion

    #region Properties

    #endregion

    public async Task Initialise(CancellationToken none) {
        MerchantDetailsModel merchantDetails = this.ApplicationCache.GetMerchantDetails();

        // TODO: handle a null
        this.Contact = merchantDetails.Contact;
    }
}