namespace TransactionMobile.Maui.BusinessLogic.ViewModels.MyAccount;

using Maui.UIServices;
using MediatR;
using MvvmHelpers;
using Services;
using TransactionMobile.Maui.BusinessLogic.Models;

public class MyAccountContactPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    private readonly IApplicationCache ApplicationCache;

    private readonly IMediator Mediator;

    private ContactModel contact;

    public ContactModel Contact {
        get => this.contact;
        set => this.SetProperty(ref this.contact, value);
    }

    #region Constructors

    public MyAccountContactPageViewModel(INavigationService navigationService, IApplicationCache applicationCache,
                                         IMediator mediator)
    {
        this.NavigationService = navigationService;
        this.ApplicationCache = applicationCache;
        this.Mediator = mediator;
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