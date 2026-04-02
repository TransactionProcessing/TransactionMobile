namespace TransactionProcessor.Mobile.BusinessLogic.UIServices;

public interface IApplicationThemeService
{
    Task ApplyConfiguredTheme();

    Task<Boolean> GetDarkThemeEnabled();

    Task SetDarkTheme(Boolean isEnabled);
}
