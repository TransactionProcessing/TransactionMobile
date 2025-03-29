using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.IntegrationTesting.Helpers;

namespace TransactionMobile.Maui.UiTests.Common
{
    using Ductus.FluentDocker;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Commands;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Executors;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using EventStore.Client;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Hosting;
    using Reqnroll;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.IntegrationTesting;
    using Shared.Logger;
    using Shouldly;
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TransactionProcessor.Client;

    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper {
        #region Fields

        /// <summary>
        /// The HTTP client
        /// </summary>
        public HttpClient HttpClient;

        /// <summary>
        /// The security service client
        /// </summary>
        public ISecurityServiceClient SecurityServiceClient;

        /// <summary>
        /// The transaction processor client
        /// </summary>
        public ITransactionProcessorClient TransactionProcessorClient;

        private const String MinimumSupportedApplicationVersion = "1.0.5";

        private readonly TestingContext TestingContext;

        #endregion

        public EventStoreProjectionManagementClient ProjectionManagementClient;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DockerHelper() : base(true) {
            this.TestingContext = new TestingContext();
        }

        #endregion

        #region Methods

        public string GetLocalIPAddress() {
            String result = String.Empty;
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ENV_IPADDRESS"))) {
                // Nothing in environment so not running under CI
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList) {
                    this.Trace($"{ip}");
                    if (ip.AddressFamily == AddressFamily.InterNetwork) {
                        result = ip.ToString();
                        break;
                    }
                }
            }
            else {
                result = Environment.GetEnvironmentVariable("ENV_IPADDRESS");
            }

            return result;
        }

        public String LocalIPAddress { get; private set; }

        public override ContainerBuilder SetupTransactionProcessorAclContainer() {
            this.AdditionalVariables.Add(ContainerType.TransactionProcessorAcl, new List<String>());
            this.SetAdditionalVariables(ContainerType.TransactionProcessorAcl, new List<String> { "AppSettings:SkipVersionCheck=true" });
            return base.SetupTransactionProcessorAclContainer();
        }

        public override async Task CreateSubscriptions() {
            List<(String streamName, String groupName, Int32 maxRetries)> subscriptions = new List<(String streamName, String groupName, Int32 maxRetries)>();
            subscriptions.AddRange(TransactionProcessor.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());

            foreach ((String streamName, String groupName, Int32 maxRetries) subscription in subscriptions) {
                var x = subscription;
                x.maxRetries = 2;
                //await this.CreatePersistentSubscription(x);
            }
        }

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName,
                                                                 DockerServices dockerServices) {
            DockerEnginePlatform engineType = BaseDockerHelper.GetDockerEnginePlatform();
            if (engineType == DockerEnginePlatform.Windows) {
                this.SetImageDetails(ContainerType.EventStore, ("stuartferguson/eventstore_windows", true));
            }

            // Get the address of the host
            this.LocalIPAddress = this.GetLocalIPAddress();
            this.Trace(this.LocalIPAddress);

            await base.StartContainersForScenarioRun(scenarioName, dockerServices);
            await SetupConfigHostContainer(this.TestNetworks);

            // Setup the base address resolvers

            HttpClientHandler clientHandler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (message,
                                                             certificate2,
                                                             arg3,
                                                             arg4) => {
                    return true;
                }
            };
            HttpClient httpClient = new HttpClient(clientHandler);
            this.SecurityServiceClient = new SecurityServiceClient(this.SecurityServiceBaseAddressResolver, httpClient);
            this.TransactionProcessorClient = new TransactionProcessorClient(this.TransactionProcessorBaseAddressResolver, httpClient);

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(this.TransactionProcessorAclBaseAddressResolver(string.Empty));

            this.TestHostHttpClient = new HttpClient(clientHandler);
            this.TestHostHttpClient.BaseAddress = new Uri($"http://127.0.0.1:{this.TestHostServicePort}");

            this.ProjectionManagementClient = new EventStoreProjectionManagementClient(ConfigureEventStoreSettings());
        }

        public String TransactionProcessorBaseAddressResolver(String api) => $"http://127.0.0.1:{this.TransactionProcessorPort}";

        public String TransactionProcessorAclBaseAddressResolver(String api) => $"http://127.0.0.1:{this.TransactionProcessorAclPort}";

        public String SecurityServiceBaseAddressResolver(String api) => $"https://127.0.0.1:{this.SecurityServicePort}";

        private async Task RemoveEstateReadModel() {
            List<Guid> estateIdList = this.TestingContext.GetAllEstateIds();

            foreach (Guid estateId in estateIdList) {
                String databaseName = $"EstateReportingReadModel{estateId}";

                // Build the connection string (to master)
                String connectionString = Setup.GetLocalConnectionString(databaseName);
                await Retry.For(async () => {
                    //EstateReportingSqlServerContext context = new EstateReportingSqlServerContext(connectionString);
                    //await context.Database.EnsureDeletedAsync(CancellationToken.None);
                }, retryFor: TimeSpan.FromMinutes(2), retryInterval: TimeSpan.FromSeconds(30));
            }
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun(DockerServices sharedDockerServices) {
            await RemoveEstateReadModel().ConfigureAwait(false);

            await base.StopContainersForScenarioRun(sharedDockerServices);
        }

        public const int ConfigHostDockerPort = 9200;

        public String ConfigHostContainerName;
        public Int32 ConfigHostPort;

        public HttpClient TestHostHttpClient;

        public async Task<IContainerService> SetupConfigHostContainer(List<INetworkService> networkServices) {
            this.Trace("About to Start Config Host Container");
            List<String> environmentVariables = new List<String>();
            environmentVariables.Add("AppSettings:InMemoryDatabase=true");
            ConfigHostContainerName = $"mobileconfighost{this.TestId:N}";

            String imageName = "stuartferguson/mobileconfiguration:latest";

            if (FdOs.IsWindows() && Shared.IntegrationTesting.DockerHelper.GetDockerEnginePlatform() == DockerEnginePlatform.Windows) {
                imageName = "stuartferguson/mobileconfigurationwindows:master";
            }

            ContainerBuilder configHostContainer = new Builder().UseContainer().WithName(ConfigHostContainerName).WithEnvironment(environmentVariables.ToArray()).UseImageDetails((imageName, true)).ExposePort(ConfigHostDockerPort).MountHostFolder(this.DockerPlatform, this.HostTraceFolder).SetDockerCredentials(this.DockerCredentials);

            // Now build and return the container                
            IContainerService builtContainer = configHostContainer.Build().Start().WaitForPort($"{ConfigHostDockerPort}/tcp", 30000);

            foreach (INetworkService networkService in networkServices) {
                networkService.Attach(builtContainer, false);
            }

            this.Trace("Config Host Container Started");
            this.Containers.Add((DockerServices.TestHost, builtContainer));

            //  Do a health check here
            this.ConfigHostPort = builtContainer.ToHostExposedEndpoint($"{ConfigHostDockerPort}/tcp").Port;

            //await this.DoHealthCheck(ContainerType.CallbackHandler);
            return builtContainer;
        }


        public override ContainerBuilder SetupTransactionProcessorContainer() {
            List<String> variables = new List<String>();
            variables.Add($"OperatorConfiguration:PataPawaPrePay:Url=http://{this.TestHostContainerName}:{DockerPorts.TestHostPort}/api/patapawaprepay");

            this.AdditionalVariables.Add(ContainerType.FileProcessor, variables);
            //this.SetAdditionalVariables(ContainerType.FileProcessor, variables);

            return base.SetupTransactionProcessorContainer();
        }

        #endregion


        public virtual async Task<IContainerService> SetupSqlServerContainerX(INetworkService networkService) {
            if (this.SqlCredentials == default)
                throw new Exception("Sql Credentials have not been set");

            IContainerService databaseServerContainer = await this.StartContainer2X(this.ConfigureSqlContainer, new List<INetworkService> { networkService }, DockerServices.SqlServer);

            return databaseServerContainer;
        }

        protected async Task<IContainerService> StartContainer2X(Func<ContainerBuilder> buildContainerFunc,
                                                                 List<INetworkService> networkServices,
                                                                 DockerServices dockerService) {
            if ((this.RequiredDockerServices & dockerService) != dockerService) {
                return default;
            }

            ConsoleStream<String> consoleLogs = null;
            try {
                ContainerBuilder containerBuilder = buildContainerFunc();

                IContainerService builtContainer = containerBuilder.Build();

                consoleLogs = builtContainer.Logs(true);
                IContainerService startedContainer = builtContainer.Start();
                foreach (INetworkService networkService in networkServices) {
                    networkService.Attach(startedContainer, false);
                }

                this.Trace($"{dockerService} Container Started");
                this.Containers.Add((dockerService, startedContainer));

                //  Do a health check here
                //this.MessagingServicePort = 
                ContainerType type = ContainerType.SqlServer;
                //await DoSqlServerHealthCheckX(startedContainer);

                this.Trace($"Container [{buildContainerFunc.Method.Name}] started");

                return startedContainer;
            }
            catch (Exception ex) {
                if (consoleLogs != null) {
                    while (consoleLogs.IsFinished == false) {
                        String s = consoleLogs.TryRead(10000);
                        this.Trace(s);
                    }
                }

                this.Error($"Error starting container [{buildContainerFunc.Method.Name}]", ex);
                throw;
            }
        }

        protected async Task DoSqlServerHealthCheckX(IContainerService containerService,
                                                     Int32 maxRetries = 20) {
            // Try opening a connection
            Int32 counter = 1;

            while (counter <= maxRetries) {
                try {
                    this.Trace($"Connection attempt {counter}");
                    CheckSqlConnectionX(containerService);
                    break;
                }
                catch (SqlException ex) {
                    this.Logger.LogError(ex);
                    await Task.Delay(30000);
                }
                finally {
                    counter++;
                }
            }

            if (counter >= maxRetries) {
                // We have got to the end and still not opened the connection
                throw new Exception($"Database container not started in {maxRetries} retries");
            }
        }

        private String sqlTestConnectionString;

        protected void CheckSqlConnectionX(IContainerService databaseServerContainer) {
            // Try opening a connection
            this.Trace("About to SQL Server Container is running");
            if (String.IsNullOrEmpty(this.sqlTestConnectionString)) {
                IPEndPoint sqlServerEndpoint = databaseServerContainer.ToHostExposedEndpoint("1433/tcp");

                //String server = "127.0.0.1";
                String server = this.GetLocalIPAddress();
                String database = "master";
                String user = this.SqlCredentials.Value.usename;
                String password = this.SqlCredentials.Value.password;
                String port = sqlServerEndpoint.Port.ToString();

                this.sqlTestConnectionString = $"server={server},{port};user id={user}; password={password}; database={database};Encrypt=False";
                this.Trace($"Connection String {this.sqlTestConnectionString}");
            }

            SqlConnection connection = new SqlConnection(this.sqlTestConnectionString);
            try {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT 1;";
                command.Prepare();
                command.ExecuteNonQuery();

                this.Trace("Connection Opened");

                connection.Close();
                this.Trace("SQL Server Container Running");
            }
            catch (SqlException ex) {
                if (connection.State == ConnectionState.Open) {
                    connection.Close();
                }

                throw;
            }
        }

        private static async Task<String> RemoveProjectionTestSetup(FileInfo file)
        {
            // Read the file
            String[] projectionLines = await File.ReadAllLinesAsync(file.FullName);

            // Find the end of the test setup code
            Int32 index = Array.IndexOf(projectionLines, "//endtestsetup");
            List<String> projectionLinesList = projectionLines.ToList();

            // Remove the test setup code
            projectionLinesList.RemoveRange(0, index + 1);
            // Rebuild the string from the lines
            String projection = String.Join(Environment.NewLine, projectionLinesList);

            return projection;
        }

        protected override EventStoreClientSettings ConfigureEventStoreSettings() {
            var ip = this.GetLocalIPAddress();
            String connectionString = $"esdb://admin:changeit@{ip}:{this.EventStoreHttpPort}";

            connectionString = this.IsSecureEventStore switch
            {
                true => $"{connectionString}?tls=true&tlsVerifyCert=false",
                _ => $"{connectionString}?tls=false&tlsVerifyCert=false"
            };

            return EventStoreClientSettings.Create(connectionString);
        }

        protected override async Task LoadEventStoreProjections() {
            //Start our Continuous Projections - we might decide to do this at a different stage, but now lets try here
            String projectionsFolder = "projections/continuous";
            IPAddress[] ipAddresses = Dns.GetHostAddresses("127.0.0.1");

            if (!String.IsNullOrWhiteSpace(projectionsFolder)) {
                DirectoryInfo di = new DirectoryInfo(projectionsFolder);

                if (di.Exists) {
                    FileInfo[] files = di.GetFiles();
                    var requiredProjections = this.GetRequiredProjections();
                    EventStoreProjectionManagementClient projectionClient = new EventStoreProjectionManagementClient(this.ConfigureEventStoreSettings());
                    List<String> projectionNames = new List<String>();

                    foreach (FileInfo file in files) {
                        if (requiredProjections.Contains(file.Name) == false)
                            continue;

                        String projection = await RemoveProjectionTestSetup(file);
                        String projectionName = file.Name.Replace(".js", String.Empty);

                        Should.NotThrow(async () => {
                                this.Trace($"Creating projection [{projectionName}] from file [{file.FullName}]");
                                try {
                                    await projectionClient.CreateContinuousAsync(projectionName, projection, trackEmittedStreams: true).ConfigureAwait(false);
                                }
                                catch (Exception ex) {
                                }

                                projectionNames.Add(projectionName);
                                this.Trace($"Projection [{projectionName}] created");
                            }, $"Projection [{projectionName}] error");
                    }

                    // Now check the create status of each
                    //foreach (String projectionName in projectionNames) {
                    //    Should.NotThrow(async () => {
                    //        ProjectionDetails projectionDetails = await projectionClient.GetStatusAsync(projectionName);

                    //        projectionDetails.Status.ShouldBe("Running", $"Projection [{projectionName}] is {projectionDetails.Status}");

                    //        this.Trace($"Projection [{projectionName}] running");
                    //    }, $"Error getting Projection [{projectionName}] status");
                    //}
                }
            }

            this.Trace("Loaded projections");
        }
    }

    public class TestingContext
    {
        #region Fields

        /// <summary>
        /// The clients
        /// </summary>
        private readonly List<ClientDetails> Clients;

        /// <summary>
        /// The estates
        /// </summary>
        public readonly List<EstateDetails> Estates;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestingContext"/> class.
        /// </summary>
        public TestingContext()
        {
            this.Estates = new List<EstateDetails>();
            this.Clients = new List<ClientDetails>();
            this.Users = new Dictionary<String, Guid>();
            this.Roles = new Dictionary<String, Guid>();
            this.ApiResources = new List<String>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public String AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the docker helper.
        /// </summary>
        /// <value>
        /// The docker helper.
        /// </value>
        public DockerHelper DockerHelper { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public NlogLogger Logger { get; set; }
        public Dictionary<String, Guid> Users;
        public Dictionary<String, Guid> Roles;
        public List<String> ApiResources;
        //public List<String> IdentityResources;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the client details.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="grantType">Type of the grant.</param>
        public void AddClientDetails(String clientId,
                                     String clientSecret,
                                     List<String> grantTypes)
        {
            this.Clients.Add(ClientDetails.Create(clientId, clientSecret, grantTypes));
        }

        /// <summary>
        /// Adds the estate details.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="estateName">Name of the estate.</param>
        public void AddEstateDetails(Guid estateId,
                                     String estateName,
                                     String estateReference)
        {
            this.Estates.Add(EstateDetails.Create(estateId, estateName, estateReference));
        }

        /// <summary>
        /// Gets all estate ids.
        /// </summary>
        /// <returns></returns>
        public List<Guid> GetAllEstateIds()
        {
            return this.Estates.Select(e => e.EstateId).ToList();
        }

        /// <summary>
        /// Gets the client details.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public ClientDetails GetClientDetails(String clientId)
        {
            ClientDetails clientDetails = this.Clients.SingleOrDefault(c => c.ClientId == clientId);

            clientDetails.ShouldNotBeNull();

            return clientDetails;
        }

        /// <summary>
        /// Gets the estate details.
        /// </summary>
        /// <param name="tableRow">The table row.</param>
        /// <returns></returns>
        public EstateDetails GetEstateDetails(DataTableRow tableRow)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");

            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        /// <summary>
        /// Gets the estate details.
        /// </summary>
        /// <param name="estateName">Name of the estate.</param>
        /// <returns></returns>
        public EstateDetails GetEstateDetails(String estateName)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        /// <summary>
        /// Gets the estate details.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <returns></returns>
        public EstateDetails GetEstateDetails(Guid estateId)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateId == estateId);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        #endregion
    }

    //public class EstateDetails
    //{
    //    #region Fields

    //    /// <summary>
    //    /// The contracts
    //    /// </summary>
    //    private readonly List<Contract> Contracts;

    //    private readonly Dictionary<String, Guid> Merchants;

    //    private readonly Dictionary<String, Dictionary<String, String>> MerchantUsers;

    //    private readonly Dictionary<String, Dictionary<String, String>> MerchantUsersTokens;

    //    private readonly Dictionary<String, Guid> Operators;

    //    #endregion

    //    #region Constructors

    //    private EstateDetails(Guid estateId,
    //                          String estateName)
    //    {
    //        this.EstateId = estateId;
    //        this.EstateName = estateName;
    //        this.Merchants = new Dictionary<String, Guid>();
    //        this.Operators = new Dictionary<String, Guid>();
    //        this.MerchantUsers = new Dictionary<String, Dictionary<String, String>>();
    //        this.MerchantUsersTokens = new Dictionary<String, Dictionary<String, String>>();
    //        this.TransactionResponses = new Dictionary<(Guid merchantId, String transactionNumber, String transactionType), String>();
    //        this.ReconciliationResponses = new Dictionary<Guid, String>();
    //        this.Contracts = new List<Contract>();
    //    }

    //    #endregion

    //    #region Properties

    //    public String AccessToken { get; private set; }

    //    public Guid EstateId { get; }

    //    public String EstateName { get; }

    //    public String EstatePassword { get; private set; }

    //    public String EstateUser { get; private set; }

    //    private Dictionary<(Guid merchantId, String transactionNumber, String transactionType), String> TransactionResponses { get; }

    //    private Dictionary<Guid, String> ReconciliationResponses { get; }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Adds the contract.
    //    /// </summary>
    //    /// <param name="contractId">The contract identifier.</param>
    //    /// <param name="contractName">Name of the contract.</param>
    //    /// <param name="operatorId">The operator identifier.</param>
    //    public void AddContract(Guid contractId,
    //                            String contractName,
    //                            Guid operatorId)
    //    {
    //        this.Contracts.Add(new Contract
    //        {
    //            ContractId = contractId,
    //            Description = contractName,
    //            OperatorId = operatorId,
    //        });
    //    }

    //    public void AddMerchant(Guid merchantId,
    //                            String merchantName)
    //    {
    //        this.Merchants.Add(merchantName, merchantId);
    //    }

    //    public void AddMerchantUser(String merchantName,
    //                                String userName,
    //                                String password)
    //    {
    //        if (this.MerchantUsers.ContainsKey(merchantName))
    //        {
    //            Dictionary<String, String> merchantUsersList = this.MerchantUsers[merchantName];
    //            if (merchantUsersList.ContainsKey(userName) == false)
    //            {
    //                merchantUsersList.Add(userName, password);
    //            }
    //        }
    //        else
    //        {
    //            Dictionary<String, String> merchantUsersList = new Dictionary<String, String>();
    //            merchantUsersList.Add(userName, password);
    //            this.MerchantUsers.Add(merchantName, merchantUsersList);
    //        }
    //    }

    //    public void AddMerchantUserToken(String merchantName,
    //                                     String userName,
    //                                     String token)
    //    {
    //        if (this.MerchantUsersTokens.ContainsKey(merchantName))
    //        {
    //            Dictionary<String, String> merchantUsersList = this.MerchantUsersTokens[merchantName];
    //            if (merchantUsersList.ContainsKey(userName) == false)
    //            {
    //                merchantUsersList.Add(userName, token);
    //            }
    //        }
    //        else
    //        {
    //            Dictionary<String, String> merchantUsersList = new Dictionary<String, String>();
    //            merchantUsersList.Add(userName, token);
    //            this.MerchantUsersTokens.Add(merchantName, merchantUsersList);
    //        }
    //    }

    //    public void AddOperator(Guid operatorId,
    //                            String operatorName)
    //    {
    //        this.Operators.Add(operatorName, operatorId);
    //    }

    //    public void AddTransactionResponse(Guid merchantId,
    //                                       String transactionNumber,
    //                                       String transactionType,
    //                                       String transactionResponse)
    //    {
    //        this.TransactionResponses.Add((merchantId, transactionNumber, transactionType), transactionResponse);
    //    }

    //    public void AddReconciliationResponse(Guid merchantId,
    //                                       String transactionResponse)
    //    {
    //        this.ReconciliationResponses.Add(merchantId, transactionResponse);
    //    }

    //    public static EstateDetails Create(Guid estateId,
    //                                       String estateName)
    //    {
    //        return new EstateDetails(estateId, estateName);
    //    }

    //    /// <summary>
    //    /// Gets the contract.
    //    /// </summary>
    //    /// <param name="contractName">Name of the contract.</param>
    //    /// <returns></returns>
    //    public Contract GetContract(String contractName)
    //    {
    //        if (this.Contracts.Any() == false)
    //        {
    //            return null;
    //        }

    //        return this.Contracts.Single(c => c.Description == contractName);
    //    }

    //    /// <summary>
    //    /// Gets the contract.
    //    /// </summary>
    //    /// <param name="contractId">The contract identifier.</param>
    //    /// <returns></returns>
    //    public Contract GetContract(Guid contractId)
    //    {
    //        return this.Contracts.Single(c => c.ContractId == contractId);
    //    }

    //    public List<Guid> GetAllMerchantIds()
    //    {
    //        return this.Merchants.Select(m => m.Value).ToList();
    //    }

    //    public Guid GetMerchantId(String merchantName)
    //    {
    //        return this.Merchants.Single(m => m.Key == merchantName).Value;
    //    }

    //    public String GetMerchantUserToken(String merchantName)
    //    {
    //        KeyValuePair<String, Dictionary<String, String>> x = this.MerchantUsersTokens.SingleOrDefault(x => x.Key == merchantName);

    //        if (x.Value != null)
    //        {
    //            return x.Value.First().Value;
    //        }

    //        return string.Empty;
    //    }

    //    public Guid GetOperatorId(String operatorName)
    //    {
    //        return this.Operators.Single(o => o.Key == operatorName).Value;
    //    }

    //    public String GetReconciliationResponse(Guid merchantId)
    //    {
    //        var reconciliationResponse =
    //            this.ReconciliationResponses
    //                .Where(t => t.Key == merchantId)
    //                .SingleOrDefault();

    //        return reconciliationResponse.Value;
    //    }

    //    public String GetTransactionResponse(Guid merchantId,
    //                                         String transactionNumber,
    //                                         String transactionType)
    //    {
    //        KeyValuePair<(Guid merchantId, String transactionNumber, String transactionType), String> transactionResponse =
    //            this.TransactionResponses
    //                .Where(t => t.Key.merchantId == merchantId && t.Key.transactionNumber == transactionNumber && t.Key.transactionType == transactionType)
    //                .SingleOrDefault();

    //        return transactionResponse.Value;
    //    }

    //    public void SetEstateUser(String userName,
    //                              String password)
    //    {
    //        this.EstateUser = userName;
    //        this.EstatePassword = password;
    //    }

    //    public void SetEstateUserToken(String accessToken)
    //    {
    //        this.AccessToken = accessToken;
    //    }

    //    #endregion
    //}

    //public class Product
    //{
    //    #region Properties

    //    /// <summary>
    //    /// Gets or sets the display text.
    //    /// </summary>
    //    /// <value>
    //    /// The display text.
    //    /// </value>
    //    public String DisplayText { get; set; }

    //    /// <summary>
    //    /// Gets or sets the name.
    //    /// </summary>
    //    /// <value>
    //    /// The name.
    //    /// </value>
    //    public String Name { get; set; }

    //    /// <summary>
    //    /// Gets or sets the product identifier.
    //    /// </summary>
    //    /// <value>
    //    /// The product identifier.
    //    /// </value>
    //    public Guid ProductId { get; set; }

    //    /// <summary>
    //    /// Gets or sets the transaction fees.
    //    /// </summary>
    //    /// <value>
    //    /// The transaction fees.
    //    /// </value>
    //    public List<TransactionFee> TransactionFees { get; set; }

    //    /// <summary>
    //    /// Gets or sets the value.
    //    /// </summary>
    //    /// <value>
    //    /// The value.
    //    /// </value>
    //    public Decimal? Value { get; set; }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Adds the transaction fee.
    //    /// </summary>
    //    /// <param name="transactionFeeId">The transaction fee identifier.</param>
    //    /// <param name="calculationType">Type of the calculation.</param>
    //    /// <param name="description">The description.</param>
    //    /// <param name="value">The value.</param>
    //    public void AddTransactionFee(Guid transactionFeeId,
    //                                  CalculationType calculationType,
    //                                  String description,
    //                                  Decimal value)
    //    {
    //        TransactionFee transactionFee = new TransactionFee
    //        {
    //            TransactionFeeId = transactionFeeId,
    //            CalculationType = calculationType,
    //            Description = description,
    //            Value = value
    //        };

    //        if (this.TransactionFees == null)
    //        {
    //            this.TransactionFees = new List<TransactionFee>();
    //        }

    //        this.TransactionFees.Add(transactionFee);
    //    }

    //    /// <summary>
    //    /// Gets the transaction fee.
    //    /// </summary>
    //    /// <param name="transactionFeeId">The transaction fee identifier.</param>
    //    /// <returns></returns>
    //    public TransactionFee GetTransactionFee(Guid transactionFeeId)
    //    {
    //        return this.TransactionFees.SingleOrDefault(t => t.TransactionFeeId == transactionFeeId);
    //    }

    //    /// <summary>
    //    /// Gets the transaction fee.
    //    /// </summary>
    //    /// <param name="description">The description.</param>
    //    /// <returns></returns>
    //    public TransactionFee GetTransactionFee(String description)
    //    {
    //        return this.TransactionFees.SingleOrDefault(t => t.Description == description);
    //    }

    //    #endregion
    //}

    public class TransactionFee
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        public CalculationType CalculationType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the transaction fee identifier.
        /// </summary>
        /// <value>
        /// The transaction fee identifier.
        /// </value>
        public Guid TransactionFeeId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Decimal Value { get; set; }

        #endregion
    }

    public class ClientDetails
    {
        public String ClientId { get; private set; }
        public String ClientSecret { get; private set; }
        public List<String> GrantTypes { get; private set; }

        private ClientDetails(String clientId,
                              String clientSecret,
                              List<String> grantTypes)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.GrantTypes = grantTypes;
        }

        public static ClientDetails Create(String clientId,
                                           String clientSecret,
                                           List<String> grantTypes)
        {
            return new ClientDetails(clientId, clientSecret, grantTypes);
        }
    }

    //public class Contract
    //{
    //    #region Properties

    //    /// <summary>
    //    /// Gets or sets the contract identifier.
    //    /// </summary>
    //    /// <value>
    //    /// The contract identifier.
    //    /// </value>
    //    public Guid ContractId { get; set; }

    //    /// <summary>
    //    /// Gets or sets the description.
    //    /// </summary>
    //    /// <value>
    //    /// The description.
    //    /// </value>
    //    public String Description { get; set; }

    //    /// <summary>
    //    /// Gets or sets the operator identifier.
    //    /// </summary>
    //    /// <value>
    //    /// The operator identifier.
    //    /// </value>
    //    public Guid OperatorId { get; set; }

    //    /// <summary>
    //    /// Gets or sets the products.
    //    /// </summary>
    //    /// <value>
    //    /// The products.
    //    /// </value>
    //    public List<Product> Products { get; set; }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Adds the product.
    //    /// </summary>
    //    /// <param name="productId">The product identifier.</param>
    //    /// <param name="name">The name.</param>
    //    /// <param name="displayText">The display text.</param>
    //    /// <param name="value">The value.</param>
    //    public void AddProduct(Guid productId,
    //                           String name,
    //                           String displayText,
    //                           Decimal? value = null)
    //    {
    //        Product product = new Product
    //        {
    //            ProductId = productId,
    //            DisplayText = displayText,
    //            Name = name,
    //            Value = value
    //        };

    //        if (this.Products == null)
    //        {
    //            this.Products = new List<Product>();
    //        }

    //        this.Products.Add(product);
    //    }

    //    /// <summary>
    //    /// Gets the product.
    //    /// </summary>
    //    /// <param name="productId">The product identifier.</param>
    //    /// <returns></returns>
    //    public Product GetProduct(Guid productId)
    //    {
    //        return this.Products.SingleOrDefault(p => p.ProductId == productId);
    //    }

    //    /// <summary>
    //    /// Gets the product.
    //    /// </summary>
    //    /// <param name="name">The name.</param>
    //    /// <returns></returns>
    //    public Product GetProduct(String name)
    //    {
    //        return this.Products.SingleOrDefault(p => p.Name == name);
    //    }

    //    #endregion
    //}
}
