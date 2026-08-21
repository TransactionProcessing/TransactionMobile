using Reqnroll;
using Shouldly;

namespace TransactionProcessor.Mobile.UITests.Common
{
    [Binding]
    [Scope(Tag = "base")]
    public class Setup
    {
        //[BeforeTestRun]
        public static Task GlobalSetup()
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(1);
            return Task.CompletedTask;
        }
    }
}
