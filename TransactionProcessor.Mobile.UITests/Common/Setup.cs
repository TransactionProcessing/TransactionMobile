using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Reqnroll;
using Shouldly;

namespace TransactionProcessor.Mobile.UITests.Common
{
    [Binding]
    [Scope(Tag = "base")]
    public class Setup{
        public static IContainerService DatabaseServerContainer;
        public static INetworkService DatabaseServerNetwork;
        public static (String usename, String password) SqlCredentials = ("sa", "thisisalongpassword123!");
        public static (String url, String username, String password) DockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");
        //[BeforeTestRun]
        public static async Task GlobalSetup(DockerHelper dockerHelper)
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(1);
            dockerHelper.SqlCredentials = Setup.SqlCredentials;
            dockerHelper.DockerCredentials = Setup.DockerCredentials;
            dockerHelper.SqlServerContainerName = "sharedsqlserver";

            Setup.DatabaseServerNetwork = dockerHelper.SetupTestNetwork("sharednetwork", true);
            Setup.DatabaseServerContainer = await dockerHelper.SetupSqlServerContainer(Setup.DatabaseServerNetwork);
        }

        public static String GetConnectionString(String databaseName)
        {
            return $"server={Setup.DatabaseServerContainer.Name};database={databaseName};user id={Setup.SqlCredentials.usename};password={Setup.SqlCredentials.password}";
        }

        public static String GetLocalConnectionString(String databaseName)
        {
            Int32 databaseHostPort = Setup.DatabaseServerContainer.ToHostExposedEndpoint("1433/tcp").Port;

            return $"server=localhost,{databaseHostPort};database={databaseName};user id={Setup.SqlCredentials.usename};password={Setup.SqlCredentials.password}";
        }

    }
}
