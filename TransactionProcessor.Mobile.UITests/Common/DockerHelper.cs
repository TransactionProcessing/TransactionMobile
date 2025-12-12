using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using EventStore.Client;
using Reqnroll;
using SecurityService.Client;
using Shared.IntegrationTesting;
using Shared.IntegrationTesting.TestContainers;
using Shared.Logger;
using Shouldly;
using TransactionProcessor.Client;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.IntegrationTesting.Helpers;
using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;

namespace TransactionProcessor.Mobile.UITests.Common
{
    public class DockerHelper : global::Shared.IntegrationTesting.TestContainers.DockerHelper
    {
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
        public DockerHelper()
        {
            this.TestingContext = new TestingContext();
        }

        #endregion

        #region Methods

        public string GetLocalIPAddress()
        {
            String result = String.Empty;
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ENV_IPADDRESS")))
            {
                // Nothing in environment so not running under CI
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    this.Trace($"{ip}");
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        result = ip.ToString();
                        break;
                    }
                }
            }
            else {
                result= Environment.GetEnvironmentVariable("ENV_IPADDRESS");
            }

            return result;
        }

        public String LocalIPAddress { get; private set; }

        public override ContainerBuilder SetupTransactionProcessorAclContainer(){
            this.AdditionalVariables.Add(ContainerType.TransactionProcessorAcl, new Dictionary<String, String> {
                {"AppSettings:SkipVersionCheck", "true"}
            });
            
            return base.SetupTransactionProcessorAclContainer();
        }

        public override async Task CreateSubscriptions()
        {
            List<(String streamName, String groupName, Int32 maxRetries)> subscriptions = new List<(String streamName, String groupName, Int32 maxRetries)>();
            subscriptions.AddRange(TransactionProcessor.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());
            
            foreach ((String streamName, String groupName, Int32 maxRetries) subscription in subscriptions)
            {
                var x = subscription;
                x.maxRetries = 2;
                await this.CreatePersistentSubscription(x);
            }
        }

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName, DockerServices dockerServices)
        {
            var engineType = await BaseDockerHelper.GetDockerEnginePlatform();
            
            if (engineType.Data == DockerEnginePlatform.Windows){
                this.SetImageDetails(ContainerType.EventStore, ("stuartferguson/eventstore_windows", true));
            }
            
            // Get the address of the host
            this.LocalIPAddress = this.GetLocalIPAddress();
            this.Trace(this.LocalIPAddress);

            await base.StartContainersForScenarioRun(scenarioName, dockerServices);
            await this.StartContainer2(this.SetupConfigHostContainer,this.TestNetworks, (DockerServices)512);

            // Setup the base address resolvers

            HttpClientHandler clientHandler = new HttpClientHandler
                                              {
                                                  ServerCertificateCustomValidationCallback = (message,
                                                                                               certificate2,
                                                                                               arg3,
                                                                                               arg4) =>
                                                                                              {
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

            this.ProjectionManagementClient = new EventStoreProjectionManagementClient(this.ConfigureEventStoreSettings());
        }

        public String TransactionProcessorBaseAddressResolver(String api) => $"http://127.0.0.1:{this.TransactionProcessorPort}";

        public String TransactionProcessorAclBaseAddressResolver(String api) => $"http://127.0.0.1:{this.TransactionProcessorAclPort}";

        public String SecurityServiceBaseAddressResolver(String api) => $"https://127.0.0.1:{this.SecurityServicePort}";
        
        public const int ConfigHostDockerPort = 9200;

        public String ConfigHostContainerName;
        public Int32 ConfigHostPort;

        public HttpClient TestHostHttpClient;

        public ContainerBuilder SetupConfigHostContainer()
        {
            this.Trace("About to Start Config Host Container");
            Dictionary<String,String> environmentVariables = new();
            environmentVariables.Add("AppSettings:InMemoryDatabase","true");

            this.ConfigHostContainerName = $"mobileconfighost{this.TestId:N}";

            String imageName = "stuartferguson/mobileconfiguration:master";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && this.DockerPlatform == DockerEnginePlatform.Windows){
                imageName = "stuartferguson/mobileconfigurationwindows:master";
            }

            ContainerBuilder configHostContainer = new ContainerBuilder().WithName(this.ConfigHostContainerName)
                .WithImage(imageName)
                .WithEnvironment(environmentVariables)
                .MountHostFolder(this.DockerPlatform, this.HostTraceFolder)
                .WithPortBinding(ConfigHostDockerPort, true);

            return configHostContainer;
        }


        //public override ContainerBuilder SetupTransactionProcessorContainer()
        //{
        //    //List<String> variables = new List<String>();
        //    //variables.Add($"OperatorConfiguration:PataPawaPrePay:Url=http://{this.TestHostContainerName}:{DockerPorts.TestHostPort}/api/patapawaprepay");

        //    //this.AdditionalVariables.Add(ContainerType.FileProcessor, variables);
        //    this.AdditionalVariables.Add(ContainerType.TransactionProcessor, new Dictionary<String, String> {
        //        {"OperatorConfiguration:PataPawaPrePay:Url", $"http://{this.TestHostContainerName}:{DockerPorts.TestHostPort}/api/patapawaprepay"}
        //    });
            
        //    return base.SetupTransactionProcessorContainer();
        //}

        #endregion
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
