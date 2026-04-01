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

        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        ResourceDictionary? existingTheme = mergedDictionaries.FirstOrDefault(d => d is LightTheme or DarkTheme);
        ResourceDictionary selectedTheme = isDarkThemeEnabled ? new DarkTheme() : new LightTheme();

        if (existingTheme == null)
        {
            mergedDictionaries.Add(selectedTheme);
            return;
        }

        ReplaceThemeDictionary(mergedDictionaries, existingTheme, selectedTheme);
    }

    private static void ReplaceThemeDictionary(ICollection<ResourceDictionary> mergedDictionaries,
                                               ResourceDictionary existingTheme,
                                               ResourceDictionary selectedTheme)
    {
        if (mergedDictionaries is IList<ResourceDictionary> dictionaryList)
        {
            Int32 themeIndex = dictionaryList.IndexOf(existingTheme);
            if (themeIndex < 0)
            {
                mergedDictionaries.Add(selectedTheme);
                return;
            }

            dictionaryList.RemoveAt(themeIndex);
            dictionaryList.Insert(themeIndex, selectedTheme);
            return;
        }

        List<ResourceDictionary> dictionaries = mergedDictionaries.ToList();
        Int32 replacementIndex = dictionaries.IndexOf(existingTheme);
        if (replacementIndex < 0)
        {
            mergedDictionaries.Add(selectedTheme);
            return;
        }

        dictionaries[replacementIndex] = selectedTheme;

        mergedDictionaries.Clear();
        foreach (ResourceDictionary resourceDictionary in dictionaries)
        {
            mergedDictionaries.Add(resourceDictionary);
        }
    }
}
