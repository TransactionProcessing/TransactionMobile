using Ductus.FluentDocker.Services;
using NLog;
using Shared.Logger;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionProcessor.Database.Entities.Summary;

namespace TransactionMobile.Maui.UiTests.Common
{
    using Ductus.FluentDocker.Services.Extensions;
    using Reqnroll;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using ILogger = Shared.Logger.ILogger;
    using Logger = Shared.Logger.Logger;

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
            Setup.DatabaseServerContainer = await dockerHelper.SetupSqlServerContainerX(Setup.DatabaseServerNetwork);
        }

        public static String GetConnectionString(String databaseName)
        {
            return $"server={Setup.DatabaseServerContainer.Name};database={databaseName};user id={Setup.SqlCredentials.usename};password={Setup.SqlCredentials.password}";
        }

        public static String GetLocalConnectionString(String databaseName)
        {
            var localIPAddress = GetLocalIPAddress();

            Int32 databaseHostPort = Setup.DatabaseServerContainer.ToHostExposedEndpoint("1433/tcp").Port;

            return $"server={localIPAddress},{databaseHostPort};database={databaseName};user id={Setup.SqlCredentials.usename};password={Setup.SqlCredentials.password}";
        }

        private static string GetLocalIPAddress()
        {
            String result = String.Empty;
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ENV_IPADDRESS")))
            {
                // Nothing in environment so not running under CI
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        result = ip.ToString();
                        break;
                    }
                }
            }
            else
            {
                result = Environment.GetEnvironmentVariable("ENV_IPADDRESS");
            }

            return result;
        }

    }
}
