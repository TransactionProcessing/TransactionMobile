namespace TransactionMobile.Maui.UIServices
{
    using BusinessLogic.UIServices;
    using CommunityToolkit.Maui.Alerts;

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
            return await Application.Current.MainPage.DisplayAlert(title, message, acceptString, cancelString);
        }

        public async Task ShowErrorToast(String message,
                                         Action action,
                                         String actionButtonText,
                                         TimeSpan? duration,
                                         CancellationToken cancellationToken) {
            await Application.Current.MainPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetInfoSnackbarOptions,
                                                               cancellationToken);
        }

        public async Task ShowInformationToast(String message,
                                               Action action,
                                               String actionButtonText,
                                               TimeSpan? duration,
                                               CancellationToken cancellationToken) {
            await Application.Current.MainPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetInfoSnackbarOptions,
                                                               cancellationToken);
        }

        public async Task<String> ShowPrompt(String title,
                                             String message,
                                             String acceptString,
                                             String cancelString,
                                             String placeHolder = "",
                                             Int32 maxLength = -1,
                                             Keyboard keyboard = null,
                                             String initialValue = "") {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, acceptString, cancelString, placeHolder, maxLength, keyboard, initialValue);
        }

        public async Task ShowWarningToast(String message,
                                           Action action,
                                           String actionButtonText,
                                           TimeSpan? duration,
                                           CancellationToken cancellationToken) {
            await Application.Current.MainPage.DisplaySnackbar(message,
                                                               action,
                                                               actionButtonText,
                                                               duration,
                                                               SnackBarOptionsHelper.GetInfoSnackbarOptions,
                                                               cancellationToken);
        }

        #endregion
    }
}