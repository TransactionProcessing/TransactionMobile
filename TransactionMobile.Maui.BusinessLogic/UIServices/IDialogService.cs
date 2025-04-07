namespace TransactionMobile.Maui.BusinessLogic.UIServices;


public interface IDialogService
{
    #region Methods

    Task ShowDialog(String title,
                    String message,
                    String acceptString);

    Task<Boolean> ShowDialog(String title,
                             String message,
                             String acceptString,
                             String cancelString);

    Task ShowErrorToast(String message,
                        Action? action = null,
                        String? actionButtonText = "OK",
                        TimeSpan? duration = null,
                        CancellationToken cancellationToken = default);

    Task ShowInformationToast(String message,
                              Action? action = null,
                              String? actionButtonText = "OK",
                              TimeSpan? duration = null,
                              CancellationToken cancellationToken = default);

    //Task<String> ShowPrompt(String title,
    //                        String message,
    //                        String acceptString,
    //                        String cancelString,
    //                        String placeHolder = "",
    //                        Int32 maxLength = -1,
    //                        Keyboard keyboard = null,
    //                        String initialValue = "");

    Task ShowWarningToast(String message,
                          Action? action = null,
                          String? actionButtonText = "OK",
                          TimeSpan? duration = null,
                          CancellationToken cancellationToken = default);

    #endregion
}