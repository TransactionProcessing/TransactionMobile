using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using EventStore.Client;
using Reqnroll;
using SecurityService.Client;
using Shared.IntegrationTesting;
using Shared.IntegrationTesting.TestContainers;
using Shared.Logger;
using Shared.Serialisation;
using Shouldly;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using TransactionProcessor.Client;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.IntegrationTesting.Helpers;
using TransactionProcessor.Mobile.UiTestBackend;
using TransactionProcessor.Mobile.UITests.Drivers;
using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;

namespace TransactionProcessor.Mobile.UITests.Common
{
    public class TestHostHelper
    {
        private readonly HttpClientHandler httpClientHandler;

        public TestHostHelper(TestingContext testingContext)
        {
            StringSerialiser.Initialise((IStringSerialiser)new SystemTextJsonSerializer(SystemTextJsonSerializer.GetDefaultJsonSerializerOptions()));
            this.TestingContext = testingContext;
            this.httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        }

        public TestingContext TestingContext { get; }

        public Guid TestId { get; private set; } = Guid.NewGuid();

        public string LocalIPAddress { get; private set; } = "127.0.0.1";

        public int ConfigHostPort { get; private set; }

        public string ConfigHostName => $"mobileconfighost{this.TestId:N}";

        public HttpClient? HttpClient { get; private set; }

        public HttpClient? TestHostHttpClient { get; private set; }

        public ISecurityServiceClient? SecurityServiceClient { get; private set; }

        public ITransactionProcessorClient? TransactionProcessorClient { get; private set; }

        public EventStoreProjectionManagementClient? ProjectionManagementClient { get; private set; }

        public NlogLogger? Logger { get; set; }

        public BackendSeed ScenarioSeed => this.TestingContext.ScenarioSeed;

        public async Task StartTestHostForScenarioRun(String scenarioName, object? _ = null)
        {
            this.TestId = Guid.NewGuid();
            this.LocalIPAddress = this.GetLocalIPAddress();
            this.TestingContext.ResetScenarioState();

            int port = GetFreeTcpPort();
            this.ConfigHostPort = port;

            this.TestingContext.BackendHost = await TestBackendHost.StartAsync(port, this.TestingContext.ScenarioSeed).ConfigureAwait(false);
            this.TestingContext.BackendHost.ApplySeed(this.TestingContext.ScenarioSeed);

            this.HttpClient = this.TestingContext.BackendHost.CreateClient();
            this.TestHostHttpClient = this.TestingContext.BackendHost.CreateClient();
            this.SecurityServiceClient = null;
            this.TransactionProcessorClient = null;
            this.ProjectionManagementClient = null;
        }

        public async Task StopTestHostForScenarioRun(object? _ = null)
        {
            if (this.TestingContext.BackendHost != null)
            {
                await this.TestingContext.BackendHost.DisposeAsync().ConfigureAwait(false);
                this.TestingContext.BackendHost = null;
            }

            this.HttpClient?.Dispose();
            this.TestHostHttpClient?.Dispose();
            this.HttpClient = null;
            this.TestHostHttpClient = null;
        }

        public string GetLocalIPAddress()
        {
            string? configuredAddress = Environment.GetEnvironmentVariable("ENV_IPADDRESS");
            if (string.IsNullOrWhiteSpace(configuredAddress) == false)
            {
                return configuredAddress;
            }

            return "127.0.0.1";
        }

        public string ConfigHostUrlForCurrentPlatform()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
            {
                return this.TestingContext.BackendHost?.AndroidBaseUrl ?? $"http://10.0.2.2:{this.ConfigHostPort}";
            }

            return this.TestingContext.BackendHost?.WindowsBaseUrl ?? $"http://127.0.0.1:{this.ConfigHostPort}";
        }

        public void ApplySeed(BackendSeed seed)
        {
            this.TestingContext.ScenarioSeed = seed.CloneSeed();
            this.TestingContext.BackendHost?.ApplySeed(this.TestingContext.ScenarioSeed);
        }

        public void SetDeviceMapping(string deviceIdentifier)
        {
            this.TestingContext.BackendHost?.SetDeviceMapping(deviceIdentifier);
        }

        public void ResetBackend()
        {
            this.TestingContext.ScenarioSeed = new BackendSeed();
            this.TestingContext.BackendHost?.Reset();
        }

        public string SecurityServiceBaseAddressResolver(string _)
        {
            return this.ConfigHostUrlForCurrentPlatform();
        }

        public string TransactionProcessorAclBaseAddressResolver(string _)
        {
            return this.ConfigHostUrlForCurrentPlatform();
        }

        public string TransactionProcessorBaseAddressResolver(string _)
        {
            return this.ConfigHostUrlForCurrentPlatform();
        }

        private static int GetFreeTcpPort()
        {
            TcpListener listener = new(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
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
            this.Roles = new Dictionary<String, String>();
            this.ApiResources = new List<String>();
            this.ScenarioSeed = new BackendSeed();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public String AccessToken { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the test host helper.
        /// </summary>
        /// <value>
        /// The test host helper.
        /// </value>
        public TestHostHelper TestHostHelper { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public NlogLogger Logger { get; set; }
        public Dictionary<String, Guid> Users;
        public Dictionary<String, String> Roles;
        public List<String> ApiResources;
        //public List<String> IdentityResources;
        public BackendSeed ScenarioSeed { get; set; }
        public bool FailureDiagnosticsCaptured { get; set; }
        public TestBackendHost? BackendHost { get; set; }

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

        public void ResetScenarioState()
        {
            this.AccessToken = String.Empty;
            this.FailureDiagnosticsCaptured = false;
            this.ScenarioSeed = new BackendSeed();
            this.Estates.Clear();
            this.Clients.Clear();
            this.Users.Clear();
            this.Roles.Clear();
            this.ApiResources.Clear();
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
