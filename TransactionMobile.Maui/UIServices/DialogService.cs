using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UIServices
{
    using BusinessLogic.UIServices;
    using CommunityToolkit.Maui.Alerts;
    using CommunityToolkit.Maui.Core;

    public class DialogService : IDialogService
    {
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

        public async Task ShowDialog(String title,
                               String message,
                               String cancelString) {

            await Application.Current.MainPage.DisplayAlert(title, message, cancelString);
        }
        public async Task<Boolean> ShowDialog(String title,
                                     String message,
                                     String acceptString,
                                     String cancelString)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, acceptString, cancelString);
        }

        public async Task<String> ShowPrompt(String title,
                                       String message,
                                       String acceptString,
                                       String cancelString,
                                       String placeHolder = "",
                                       Int32 maxLength = -1,
                                       Keyboard keyboard = null,
                                       String initialValue = "") {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, acceptString, cancelString, placeHolder, maxLength,keyboard,initialValue);
        }
    }

    public static class SnackBarOptionsHelper
    {
        public static SnackbarOptions GetInfoSnackbarOptions => new SnackbarOptions {
                                                                                        BackgroundColor = Colors.Blue,
                                                                                        TextColor = Colors.White
                                                                                    };

        public static SnackbarOptions GetWarningSnackbarOptions => new SnackbarOptions
                                                                   {
            BackgroundColor = Colors.Orange,
            TextColor = Colors.White
        };

        public static SnackbarOptions GetErrorSnackbarOptions => new SnackbarOptions
                                                                 {
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
                                                                 };
    }
}
