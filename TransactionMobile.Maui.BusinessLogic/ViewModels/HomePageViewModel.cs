namespace TransactionMobile.Maui.BusinessLogic.ViewModels;

using System.Diagnostics.CodeAnalysis;
using Logging;
using Maui.UIServices;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;
using Microsoft.Maui.Devices;
using Models;
using Services;
using UIServices;

[ExcludeFromCodeCoverage]
public class HomePageViewModel : ExtendedBaseViewModel
{
    #region Constructors
    
    public HomePageViewModel(IApplicationCache applicationCache,
                             IDialogService dialogService,
                             IDeviceService deviceService,
                             INavigationService navigationService) :base(applicationCache,dialogService, navigationService, deviceService)
    {
        
    }

    public async Task ShowDebugMessage(String message) {
        Configuration configuration = this.ApplicationCache.GetConfiguration();

        if (configuration.ShowDebugMessages)
        {
            await this.DialogService.ShowDialog("Debug", message, "OK");
        }
    }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken) {
        Configuration configuration = this.ApplicationCache.GetConfiguration();

        if (configuration.EnableAutoUpdates) {
            await Distribute.SetEnabledAsync(true);
            Distribute.ReleaseAvailable = this.OnReleaseAvailable;
            Distribute.NoReleaseAvailable = NoReleaseAvailable;
            Distribute.UpdateTrack = UpdateTrack.Public;
            Distribute.CheckForUpdate();
        }
        else {
            Distribute.DisableAutomaticCheckForUpdate();
        }

        try {
            if (this.IsIOS() == false) {
                AppCenter.Start(configuration.AppCenterConfig.GetAppCenterKey(), typeof(Distribute));
            }
        }
        catch(Exception ex) {
            Logger.LogError("Error during initialise", ex);
        }
    }

    private async void NoReleaseAvailable() {
        await this.DialogService.ShowDialog("Software Update", "No Release Available","OK");
    }

    private Boolean IsIOS() => DeviceInfo.Current.Platform == DevicePlatform.iOS;

    private Boolean OnReleaseAvailable(ReleaseDetails releaseDetails) {
        Logger.LogInformation("In OnReleaseAvailable");
        // Look at releaseDetails public properties to get version information, release notes text or release notes URL
        String versionName = releaseDetails.ShortVersion;
        String versionCodeOrBuildNumber = releaseDetails.Version;
        String releaseNotes = releaseDetails.ReleaseNotes;
        Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

        // custom dialog
        String title = "Version " + versionName + " available!";
        Task answer;

        // On mandatory update, user can't postpone
        if (releaseDetails.MandatoryUpdate)
        {
            Logger.LogInformation("In OnReleaseAvailable - mandatory update");
            answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install");
        }
        else
        {
            Logger.LogInformation("In OnReleaseAvailable - non mandatory update");
            answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install", "Later");
        }

        answer.ContinueWith(task =>
        {
            // If mandatory or if answer was positive
            if (releaseDetails.MandatoryUpdate || (task as Task<Boolean>).Result)
            {
                Logger.LogInformation("In OnReleaseAvailable - updating");
                // Notify SDK that user selected update
                Distribute.NotifyUpdateAction(UpdateAction.Update);
            }
            else
            {
                // Notify SDK that user selected postpone (for 1 day)
                // This method call is ignored by the SDK if the update is mandatory
                Logger.LogInformation("In OnReleaseAvailable - postponing");
                Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }
        });

        // Return true if you're using your own dialog, false otherwise
        return true;
    }

    #endregion
}