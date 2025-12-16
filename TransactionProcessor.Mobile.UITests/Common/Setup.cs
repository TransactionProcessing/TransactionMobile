using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Reqnroll;
using Shouldly;

namespace TransactionProcessor.Mobile.UITests.Common
{
    [Binding]
    [Scope(Tag = "base")]
    public class Setup{
        public static (String usename, String password) SqlCredentials = ("sa", "thisisalongpassword123!");
        public static (String url, String username, String password) DockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");
        //[BeforeTestRun]
        public static async Task GlobalSetup(DockerHelper dockerHelper)
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(1);
        }
    }
}
