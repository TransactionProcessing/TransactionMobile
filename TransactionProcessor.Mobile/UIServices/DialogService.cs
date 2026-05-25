using CommunityToolkit.Maui.Alerts;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.UIServices
{
    public class DialogService : IDialogService
    {
        #region Methods

        public async Task ShowDialog(String title,
                                     String message,
                                     String cancelString) {
            await Application.Current.MainPage.DisplayAlert(title, message, cancelString);
        }

        public async Task<Boolean> ShowDialog(String title,
                                              String message,
                                              String acceptString,
                                              String cancelString) {
            await Task.Delay(200); // allow view to fully appear
            return await Application.Current.MainPage.DisplayAlert(title, message, acceptString, cancelString);
        }

        public async Task ShowErrorToast(String message,
                                         Action? action = null,
                                         String? actionButtonText = "OK",
                                         TimeSpan? duration = null,
                                         CancellationToken cancellationToken = default)
        {
            await Application.Current.MainPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetErrorSnackbarOptions,
                                                               cancellationToken);
        }

        public async Task ShowInformationToast(String message,
                                               Action? action = null,
                                               String? actionButtonText = "OK",
                                               TimeSpan? duration = null,
                                               CancellationToken cancellationToken = default)
        {
            await Application.Current.MainPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetInfoSnackbarOptions,
                                                               cancellationToken);
        }

        public async Task ShowWarningToast(String message,
                                           Action? action = null,
                                           String? actionButtonText = "OK",
                                           TimeSpan? duration = null,
                                           CancellationToken cancellationToken = default)
        {
            if (duration == null)
            {
                duration = TimeSpan.FromSeconds(10);
            }
            await Application.Current.MainPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetWarningSnackbarOptions,
                                                               cancellationToken);
        }

        #endregion
    }
}