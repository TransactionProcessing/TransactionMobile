using Microsoft.Maui.Graphics;

namespace TransactionMobile.Maui.UIServices;

using CommunityToolkit.Maui.Core;

public static class SnackBarOptionsHelper
{
    #region Properties

    public static SnackbarOptions GetErrorSnackbarOptions =>
        new SnackbarOptions {
                                BackgroundColor = Colors.Red,
                                TextColor = Colors.White,
                            };

    public static SnackbarOptions GetInfoSnackbarOptions =>
        new SnackbarOptions {
                                BackgroundColor = Colors.Blue,
                                TextColor = Colors.White
                            };

    public static SnackbarOptions GetWarningSnackbarOptions =>
        new SnackbarOptions {
                                BackgroundColor = Colors.Orange,
                                TextColor = Colors.White
                            };

    #endregion
}