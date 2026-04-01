namespace TransactionProcessor.Mobile.Pages.Common;

internal static class ThemeButtonImageSource
{
    public static String Get(String imageSource)
    {
        if (Application.Current?.RequestedTheme != AppTheme.Dark)
        {
            return imageSource;
        }

        String imageExtension = Path.GetExtension(imageSource);
        String imageName = Path.GetFileNameWithoutExtension(imageSource);

        return $"{imageName}-dark{imageExtension}";
    }
}
