using CommunityToolkit.Maui.Alerts;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.UIServices
{
    public class DialogService : IDialogService
    {
        private static Page CurrentPage => Application.Current?.Windows.FirstOrDefault()?.Page
            ?? throw new InvalidOperationException("No active application window is available.");

        #region Methods

        public async Task ShowDialog(String title,
                                     String message,
                                     String cancelString) {
            await CurrentPage.DisplayAlertAsync(title, message, cancelString);
        }

        public async Task<Boolean> ShowDialog(String title,
                                              String message,
                                              String acceptString,
                                              String cancelString) {
            await Task.Delay(200); // allow view to fully appear
            return await CurrentPage.DisplayAlertAsync(title, message, acceptString, cancelString);
        }

        public async Task ShowErrorToast(String message,
                                         Action? action = null,
                                         String? actionButtonText = "OK",
                                         TimeSpan? duration = null,
                                         CancellationToken cancellationToken = default)
        {
            await CurrentPage.DisplaySnackbar(message,
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
            await CurrentPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                                SnackBarOptionsHelper.GetInfoSnackbarOptions,
                                                                cancellationToken);
        }

        public async Task ShowUploadLogsCompleteNotice()
        {
            if (OperatingSystem.IsWindows())
            {
                await CurrentPage.DisplayAlertAsync(
                    "Logs uploaded",
                    "Logs have been uploaded successfully.",
                    "OK");
                return;
            }

            await CurrentPage.DisplaySnackbar(
                "Logs have been uploaded successfully.",
                null,
                "OK",
                null,
                SnackBarOptionsHelper.GetSuccessSnackbarOptions);
        }

        public async Task ShowSuccessToast(String message,
                                           Action? action = null,
                                           String? actionButtonText = "OK",
                                           TimeSpan? duration = null,
                                           CancellationToken cancellationToken = default)
        {
            await CurrentPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetSuccessSnackbarOptions,
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
            await CurrentPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetWarningSnackbarOptions,
                                                               cancellationToken);
        }

        #endregion
    }
}
