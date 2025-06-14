namespace TransactionMobile.Maui.BusinessLogic.ViewModels;

using System.Diagnostics.CodeAnalysis;
using Logging;
using Maui.UIServices;
using Models;
using Services;
using UIServices;

[ExcludeFromCodeCoverage]
public class HomePageViewModel : ExtendedBaseViewModel
{
    public HomePageViewModel(IApplicationCache applicationCache,
                             IDialogService dialogService,
                             IDeviceService deviceService,
                             INavigationService navigationService,
                             INavigationParameterService navigationParameterService) :base(applicationCache,dialogService, navigationService, deviceService,navigationParameterService)
    {
        
    }
}