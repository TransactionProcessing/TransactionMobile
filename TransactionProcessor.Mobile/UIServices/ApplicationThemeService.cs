using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.Resources.Styles;

namespace TransactionProcessor.Mobile.UIServices;

public class ApplicationThemeService : IApplicationThemeService
{
    private const String DarkThemeEnabledOptionName = "DarkThemeEnabled";

    private readonly IDatabaseContext DatabaseContext;

    public ApplicationThemeService(IDatabaseContext databaseContext)
    {
        this.DatabaseContext = databaseContext;
    }

    public async Task ApplyConfiguredTheme()
    {
        Boolean isDarkThemeEnabled = await this.GetDarkThemeEnabled();
        this.ApplyTheme(isDarkThemeEnabled);
    }

    public async Task<Boolean> GetDarkThemeEnabled()
    {
        String? optionValue = await this.DatabaseContext.GetApplicationOption(DarkThemeEnabledOptionName);

        return Boolean.TryParse(optionValue, out Boolean isDarkThemeEnabled) && isDarkThemeEnabled;
    }

    public async Task SetDarkTheme(Boolean isEnabled)
    {
        await this.DatabaseContext.SaveApplicationOption(DarkThemeEnabledOptionName, isEnabled.ToString());
        this.ApplyTheme(isEnabled);
    }

    private void ApplyTheme(Boolean isDarkThemeEnabled)
    {
        if (Application.Current == null)
        {
            return;
        }

        Application.Current.UserAppTheme = isDarkThemeEnabled ? AppTheme.Dark : AppTheme.Light;

        IList<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        ResourceDictionary? existingTheme = mergedDictionaries.FirstOrDefault(d => d is LightTheme or DarkTheme);
        ResourceDictionary selectedTheme = isDarkThemeEnabled ? new DarkTheme() : new LightTheme();

        if (existingTheme == null)
        {
            mergedDictionaries.Add(selectedTheme);
            return;
        }

        Int32 themeIndex = mergedDictionaries.IndexOf(existingTheme);
        mergedDictionaries.RemoveAt(themeIndex);
        mergedDictionaries.Insert(themeIndex, selectedTheme);
    }
}
