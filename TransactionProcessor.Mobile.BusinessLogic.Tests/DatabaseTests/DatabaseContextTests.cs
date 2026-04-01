using TransactionProcessor.Mobile.BusinessLogic.Database;
using Shouldly;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.DatabaseTests;

public class DatabaseContextTests
{
    [Fact]
    public async Task DatabaseContext_SaveApplicationOption_OptionCanBeReadBack()
    {
        Func<LogLevel> logLevelFunc = () => LogLevel.Debug;
        IDatabaseContext databaseContext = new DatabaseContext(":memory:", logLevelFunc);

        await databaseContext.InitialiseDatabase();
        await databaseContext.SaveApplicationOption("DarkThemeEnabled", "True");

        String? optionValue = await databaseContext.GetApplicationOption("DarkThemeEnabled");

        optionValue.ShouldBe("True");
    }

    [Fact]
    public async Task DatabaseContext_SaveApplicationOption_ExistingValueIsUpdated()
    {
        Func<LogLevel> logLevelFunc = () => LogLevel.Debug;
        IDatabaseContext databaseContext = new DatabaseContext(":memory:", logLevelFunc);

        await databaseContext.InitialiseDatabase();
        await databaseContext.SaveApplicationOption("DarkThemeEnabled", "True");
        await databaseContext.SaveApplicationOption("DarkThemeEnabled", "False");

        String? optionValue = await databaseContext.GetApplicationOption("DarkThemeEnabled");

        optionValue.ShouldBe("False");
    }
}
