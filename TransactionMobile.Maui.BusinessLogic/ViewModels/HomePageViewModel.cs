namespace TransactionMobile.Maui.BusinessLogic.ViewModels;

using System.Runtime.CompilerServices;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;
using Models;
using MvvmHelpers;
using Services;
using UIServices;

public class HomePageViewModel : BaseViewModel
{
    #region Fields

    private readonly IApplicationCache ApplicationCache;

    private readonly IDialogService DialogService;

    #endregion

    #region Constructors

    public HomePageViewModel(IApplicationCache applicationCache,
                             IDialogService dialogService) {
        this.ApplicationCache = applicationCache;
        this.DialogService = dialogService;
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
            Shared.Logger.Logger.LogError(ex);
        }
    }

    private async void NoReleaseAvailable() {
        await this.DialogService.ShowDialog("Software Update", "No Release Avaiable","OK");
    }

    private Boolean IsIOS() => DeviceInfo.Current.Platform == DevicePlatform.iOS;

    private Boolean OnReleaseAvailable(ReleaseDetails releaseDetails) {
        Shared.Logger.Logger.LogInformation("In OnReleaseAvailable");
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
            Shared.Logger.Logger.LogInformation("In OnReleaseAvailable - mandatory update");
            answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install");
        }
        else
        {
            Shared.Logger.Logger.LogInformation("In OnReleaseAvailable - non mandatory update");
            answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install", "Later");
        }

        answer.ContinueWith(task =>
        {
            // If mandatory or if answer was positive
            if (releaseDetails.MandatoryUpdate || (task as Task<Boolean>).Result)
            {
                Shared.Logger.Logger.LogInformation("In OnReleaseAvailable - updating");
                // Notify SDK that user selected update
                Distribute.NotifyUpdateAction(UpdateAction.Update);
            }
            else
            {
                // Notify SDK that user selected postpone (for 1 day)
                // This method call is ignored by the SDK if the update is mandatory
                Shared.Logger.Logger.LogInformation("In OnReleaseAvailable - postponing");
                Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }
        });

        // Return true if you're using your own dialog, false otherwise
        return true;
    }

    #endregion
}