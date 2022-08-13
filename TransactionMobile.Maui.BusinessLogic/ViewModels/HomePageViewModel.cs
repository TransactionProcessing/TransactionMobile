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

    private readonly IDialogService DialogService;

    private readonly IMemoryCacheService MemoryCacheService;

    #endregion

    #region Constructors

    public HomePageViewModel(IMemoryCacheService memoryCacheService,
                             IDialogService dialogService) {
        this.MemoryCacheService = memoryCacheService;
        this.DialogService = dialogService;
    }

    public async Task ShowDebugMessage(String message) {
        this.MemoryCacheService.TryGetValue("Configuration", out Configuration configuration);

        if (configuration.ShowDebugMessages) {
            await this.DialogService.ShowDialog("Debug", message, "OK");
        }
    }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken) {
        this.MemoryCacheService.TryGetValue("Configuration", out Configuration configuration);

        if (configuration.EnableAutoUpdates) {
            await ShowDebugMessage("EnableAutoUpdates is true");
            await Distribute.SetEnabledAsync(true);
            await ShowDebugMessage("Distibute Enabled Set");
            Distribute.ReleaseAvailable = this.OnReleaseAvailable;
            Distribute.NoReleaseAvailable = NoReleaseAvailable;
            Distribute.UpdateTrack = UpdateTrack.Public;
            await ShowDebugMessage("Calling Check for update");
            Distribute.CheckForUpdate();
            
            await ShowDebugMessage("CheckForUpdate called");

        }
        else {
            Distribute.DisableAutomaticCheckForUpdate();
        }

        try {
            if (this.IsIOS() == false) {
                // TODO: Move the keys to config service
                AppCenter.Start("android=f920cc96-de56-42fe-87d4-b49105761205;" + "ios=dd940171-ca8c-4219-9851-f83769464f37;" +
                                "uwp=3ad27ea3-3f24-4579-a88a-530025bd00d4;" + "macos=244fdee2-f897-431a-8bab-5081fc90b329;",
                                typeof(Distribute));
            }
        }
        catch(Exception ex) {
            await ShowDebugMessage(ex.Message);
        }
    }

    private async void NoReleaseAvailable() {
        await this.DialogService.ShowDialog("Software Update", "No Release Avaiable","OK");
    }

    private Boolean IsIOS() => DeviceInfo.Current.Platform == DevicePlatform.iOS;

    private Boolean OnReleaseAvailable(ReleaseDetails releaseDetails) {
        // Look at releaseDetails public properties to get version information, release notes text or release notes URL
        String versionName = releaseDetails.ShortVersion;
        String versionCodeOrBuildNumber = releaseDetails.Version;
        String releaseNotes = releaseDetails.ReleaseNotes;
        Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

        // custom dialog
        String title = "Version " + versionName + " available!";
        Task answer;

        // On mandatory update, user can't postpone
        if (releaseDetails.MandatoryUpdate) {
            answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install");
        }
        else {
            answer = this.DialogService.ShowDialog(title, releaseNotes, "Download and Install", "Later");
        }

        answer.ContinueWith(task => {
                                // If mandatory or if answer was positive
                                if (releaseDetails.MandatoryUpdate || (task as Task<Boolean>).Result) {
                                    // Notify SDK that user selected update
                                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                                }
                                else {
                                    // Notify SDK that user selected postpone (for 1 day)
                                    // This method call is ignored by the SDK if the update is mandatory
                                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                                }
                            });

        // Return true if you're using your own dialog, false otherwise
        return true;
    }

    #endregion
}