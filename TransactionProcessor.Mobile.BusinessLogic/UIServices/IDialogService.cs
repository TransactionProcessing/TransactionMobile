namespace TransactionProcessor.Mobile.BusinessLogic.UIServices;


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

    Task ShowSuccessToast(String message,
                          Action? action = null,
                          String? actionButtonText = "OK",
                          TimeSpan? duration = null,
                          CancellationToken cancellationToken = default);

    Task ShowWarningToast(String message,
                          Action? action = null,
                          String? actionButtonText = "OK",
                          TimeSpan? duration = null,
                          CancellationToken cancellationToken = default);

    #endregion
}
