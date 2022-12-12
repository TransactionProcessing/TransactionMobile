using Ductus.FluentDocker.Services;
using NLog;
using Shared.Logger;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace TransactionMobile.Maui.UiTests.Common
{
    using Ductus.FluentDocker.Services.Extensions;

    [Binding]
    public class Setup
    {
        public static IContainerService DatabaseServerContainer;
        public static INetworkService DatabaseServerNetwork;
        public static (String usename, String password) SqlCredentials = ("sa", "thisisalongpassword123!");
        public static (String url, String username, String password) DockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");
        [BeforeTestRun]
        protected static void GlobalSetup()
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(1);

            DockerHelper dockerHelper = new DockerHelper();

            NlogLogger logger = new NlogLogger();
            logger.Initialise(LogManager.GetLogger("Specflow"), "Specflow");
            LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);
            dockerHelper.Logger = logger;
            dockerHelper.SqlCredentials = Setup.SqlCredentials;
            dockerHelper.DockerCredentials = Setup.DockerCredentials;
            dockerHelper.SqlServerContainerName = "sharedsqlserver";

            Setup.DatabaseServerNetwork = dockerHelper.SetupTestNetwork("sharednetwork", true);
            Setup.DatabaseServerContainer = dockerHelper.SetupSqlServerContainer(Setup.DatabaseServerNetwork);
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
