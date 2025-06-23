using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionProcessor.Database.Contexts;
using TransactionProcessor.DataTransferObjects.Requests.Contract;
using TransactionProcessor.DataTransferObjects.Requests.Estate;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.Operator;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using AssignOperatorRequest = TransactionProcessor.DataTransferObjects.Requests.Estate.AssignOperatorRequest;

namespace TransactionMobile.Maui.UiTests.Steps
{
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using Common;
    using Drivers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Reqnroll;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;
    using SecurityService.IntegrationTesting.Helpers;
    using Shared.IntegrationTesting;
    using Shouldly;
    using TransactionProcessor.DataTransferObjects;
    using TransactionProcessor.IntegrationTesting.Helpers;
    using UITests;
    using ClientDetails = Common.ClientDetails;
    using ReqnrollExtensions = TransactionProcessor.IntegrationTesting.Helpers.ReqnrollExtensions;
    using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly TestingContext TestingContext;

        private readonly SecurityServiceSteps SecurityServiceSteps;

        private readonly TransactionProcessorSteps TransactionProcessorSteps;

        private LoginPage loginPage;

        public SharedSteps(TestingContext testingContext) {
            this.TestingContext = testingContext;
            this.loginPage = new LoginPage(testingContext);
            this.SecurityServiceSteps = new SecurityServiceSteps(testingContext.DockerHelper.SecurityServiceClient);
            this.TransactionProcessorSteps = new TransactionProcessorSteps(this.TestingContext.DockerHelper.TransactionProcessorClient, this.TestingContext.DockerHelper.TestHostHttpClient,
                this.TestingContext.DockerHelper.ProjectionManagementClient);
        }

        [Given(@"the following security roles exist")]
        public async Task GivenTheFollowingSecurityRolesExist(DataTable table)
        {
            List<CreateRoleRequest> requests = table.Rows.ToCreateRoleRequests();
            List<(String, Guid)> responses = await this.SecurityServiceSteps.GivenICreateTheFollowingRoles(requests, CancellationToken.None);

            foreach ((String, Guid) response in responses)
            {
                this.TestingContext.Roles.Add(response.Item1, response.Item2);
            }
        }

        [Given(@"I create the following api scopes")]
        public async Task GivenICreateTheFollowingApiScopes(DataTable table) {
            List<CreateApiScopeRequest> requests = table.Rows.ToCreateApiScopeRequests();
            await this.SecurityServiceSteps.GivenICreateTheFollowingApiScopes(requests);
        }

        [Given(@"the following api resources exist")]
        public async Task GivenTheFollowingApiResourcesExist(DataTable table)
        {
            List<CreateApiResourceRequest> requests = table.Rows.ToCreateApiResourceRequests();
            await this.SecurityServiceSteps.GivenTheFollowingApiResourcesExist(requests);

            foreach (CreateApiResourceRequest createApiResourceRequest in requests)
            {
                this.TestingContext.ApiResources.Add(createApiResourceRequest.Name);
            }
        }

        [Given(@"the following clients exist")]
        public async Task GivenTheFollowingClientsExist(DataTable table)
        {
            List<CreateClientRequest> requests = table.Rows.ToCreateClientRequests(this.TestingContext.DockerHelper.TestId);
            List<(String clientId, String secret, List<String> allowedGrantTypes)> clients = await this.SecurityServiceSteps.GivenTheFollowingClientsExist(requests);
            foreach ((String clientId, String secret, List<String> allowedGrantTypes) client in clients)
            {
                this.TestingContext.AddClientDetails(client.clientId, client.secret, client.allowedGrantTypes);
            }
        }

        [Given(@"I have a token to access the estate management and transaction processor acl resources")]
        public async Task GivenIHaveATokenToAccessTheEstateManagementAndTransactionProcessorAclResources(DataTable table)
        {
            DataTableRow firstRow = table.Rows.First();
            String clientId = ReqnrollTableHelper.GetStringRowValue(firstRow, "ClientId").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
            ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

            this.TestingContext.AccessToken = await this.SecurityServiceSteps.GetClientToken(clientDetails.ClientId, clientDetails.ClientSecret, CancellationToken.None);
        }
        
        [Given(@"I have created the following estates")]
        public async Task GivenIHaveCreatedTheFollowingEstates(DataTable table) {
            List<CreateEstateRequest> requests = table.Rows.ToCreateEstateRequests();
            
            List<EstateResponse> verifiedEstates = await this.TransactionProcessorSteps.WhenICreateTheFollowingEstatesX(this.TestingContext.AccessToken, requests);

            foreach (EstateResponse verifiedEstate in verifiedEstates)
            {

                await Retry.For(async () => {
                                    String databaseName = $"EstateReportingReadModel{verifiedEstate.EstateId}";
                                    var connString = Setup.GetLocalConnectionString(databaseName);
                                    connString = $"{connString};Encrypt=false";
                                    var ctx = new EstateManagementSqlServerContext(connString);

                                    var estates = ctx.Estates.ToList();
                                    estates.Count.ShouldBe(1);

                                    this.TestingContext.AddEstateDetails(verifiedEstate.EstateId, verifiedEstate.EstateName, verifiedEstate.EstateReference);
                                    this.TestingContext.Logger.LogInformation($"Estate {verifiedEstate.EstateName} created with Id {verifiedEstate.EstateId}");
                                }, TimeSpan.FromMinutes(2));
            }
        }

        [Given(@"I have created the following operators")]
        public async Task GivenIHaveCreatedTheFollowingOperators(DataTable table)
        {
            List<(EstateDetails estate, CreateOperatorRequest request)> requests = table.Rows.ToCreateOperatorRequests(this.TestingContext.Estates);

            List<(Guid, EstateOperatorResponse)> results = await this.TransactionProcessorSteps.WhenICreateTheFollowingOperators(this.TestingContext.AccessToken, requests);

            foreach ((Guid, EstateOperatorResponse) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Operator {result.Item2.Name} created with Id {result.Item2.OperatorId} for Estate {result.Item1}");
            }
        }

        [Given("I have assigned the following operators to the estates")]
        public async Task GivenIHaveAssignedTheFollowingOperatorsToTheEstates(DataTable dataTable)
        {
            List<(EstateDetails estate, AssignOperatorRequest request)> requests = dataTable.Rows.ToAssignOperatorToEstateRequests(this.TestingContext.Estates);

            await this.TransactionProcessorSteps.GivenIHaveAssignedTheFollowingOperatorsToTheEstates(this.TestingContext.AccessToken, requests);

            // TODO Verify
        }


        [Given(@"I create a contract with the following values")]
        public async Task GivenICreateAContractWithTheFollowingValues(DataTable table)
        {
            var estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails, CreateContractRequest)> requests = table.Rows.ToCreateContractRequests(estates);
            List<ContractResponse> responses = await this.TransactionProcessorSteps.GivenICreateAContractWithTheFollowingValues(this.TestingContext.AccessToken, requests);
            foreach (ContractResponse contractResponse in responses)
            {
                EstateDetails estate = this.TestingContext.Estates.Single(e => e.EstateId == contractResponse.EstateId);
                estate.AddContract(contractResponse.ContractId, contractResponse.Description, contractResponse.OperatorId);
            }
        }

        [When(@"I create the following Products")]
        public async Task WhenICreateTheFollowingProducts(Table table)
        {
            var estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails, Contract, AddProductToContractRequest)> requests = table.Rows.ToAddProductToContractRequest(estates);
            await this.TransactionProcessorSteps.WhenICreateTheFollowingProducts(this.TestingContext.AccessToken, requests);
        }

        [When(@"I add the following Transaction Fees")]
        public async Task WhenIAddTheFollowingTransactionFees(DataTable table)
        {
            var estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> requests = table.Rows.ToAddTransactionFeeForProductToContractRequests(estates);
            await this.TransactionProcessorSteps.WhenIAddTheFollowingTransactionFees(this.TestingContext.AccessToken, requests);
        }

        [Given(@"I create the following merchants")]
        public async Task GivenICreateTheFollowingMerchants(DataTable table)
        {
            var estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails estate, CreateMerchantRequest)> requests = table.Rows.ToCreateMerchantRequests(estates);

            List<MerchantResponse> verifiedMerchants = await this.TransactionProcessorSteps.WhenICreateTheFollowingMerchants(this.TestingContext.AccessToken, requests);

            foreach (MerchantResponse verifiedMerchant in verifiedMerchants)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(verifiedMerchant.EstateId);
                estateDetails.AddMerchant(verifiedMerchant);
                this.TestingContext.Logger.LogInformation($"Merchant {verifiedMerchant.MerchantName} created with Id {verifiedMerchant.MerchantId} for Estate {estateDetails.EstateName}");
            }
        }

        [Given(@"I have assigned the following  operator to the merchants")]
        public async Task GivenIHaveAssignedTheFollowingOperatorToTheMerchants(DataTable table)
        {
            List<EstateDetails> estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails, Guid, TransactionProcessor.DataTransferObjects.Requests.Merchant.AssignOperatorRequest)> requests = table.Rows.ToAssignOperatorRequests(estates);

            List<(EstateDetails, MerchantOperatorResponse)> results = await this.TransactionProcessorSteps.WhenIAssignTheFollowingOperatorToTheMerchants(this.TestingContext.AccessToken, requests);

            foreach ((EstateDetails, MerchantOperatorResponse) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Operator {result.Item2.Name} assigned to Estate {result.Item1.EstateName}");
            }
        }
        
        [Given(@"I have created a config for my device")]
        public async Task GivenIHaveCreatedAConfigForMyDevice() {
            var deviceSerial = await this.loginPage.GetDeviceSerial();
            
            var clientDetails = this.TestingContext.GetClientDetails("mobileAppClient");
            var configRequest = new {
                                        clientId = clientDetails.ClientId,
                                        clientSecret = clientDetails.ClientSecret,
                                        deviceIdentifier = deviceSerial,
                                        id = deviceSerial,
                                        enableAutoUpdates = false,
                                        logLevel = 3,
                                        hostAddresses = new List<Object>()
                                    };
            configRequest.hostAddresses.Add(new {
                                                    servicetype = 0,
                                                    uri = this.TestingContext.DockerHelper.TransactionProcessorBaseAddressResolver("").Replace("127.0.0.1", this.TestingContext.DockerHelper.LocalIPAddress)
            });
            configRequest.hostAddresses.Add(new
                                            {
                                                servicetype = 1,
                                                uri = this.TestingContext.DockerHelper.SecurityServiceBaseAddressResolver("").Replace("127.0.0.1", this.TestingContext.DockerHelper.LocalIPAddress)
            });
            configRequest.hostAddresses.Add(new
                                            {
                                                servicetype = 2,
                                                uri = this.TestingContext.DockerHelper.TransactionProcessorAclBaseAddressResolver("").Replace("127.0.0.1", this.TestingContext.DockerHelper.LocalIPAddress)
            });

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"http://127.0.0.1:{this.TestingContext.DockerHelper.ConfigHostPort}/api/transactionmobileconfiguration");
            request.Content = new StringContent(JsonConvert.SerializeObject(configRequest), Encoding.UTF8, "application/json");

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

            var response = await httpClient.SendAsync(request, CancellationToken.None);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            this.TestingContext.Logger.LogInformation($"Config Created for serial {deviceSerial}");
        }

        [Given(@"I have created a config for my application")]
        public async Task GivenIHaveCreatedAConfigForMyApplication()
        {
            var configRequest = new
            {
                applicationId = "transactionMobilePOS",
                androidkey = "android",
                ioskey = "ios",
                macoskey = "macos",
                windowskey = "windows",
            };
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"http://127.0.0.1:{this.TestingContext.DockerHelper.ConfigHostPort}/api/applicationcentreconfiguration");
            request.Content = new StringContent(JsonConvert.SerializeObject(configRequest), Encoding.UTF8, "application/json");

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

            var response = await httpClient.SendAsync(request, CancellationToken.None);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private async Task<Decimal> GetMerchantBalance(Guid merchantId)
        {
            JsonElement jsonElement = (JsonElement)await this.TestingContext.DockerHelper.ProjectionManagementClient.GetStateAsync<dynamic>("MerchantBalanceProjection", $"MerchantBalance-{merchantId:N}");
            JObject jsonObject = JObject.Parse(jsonElement.GetRawText());
            decimal balanceValue = jsonObject.SelectToken("merchant.balance").Value<decimal>();
            return balanceValue;
        }

        [Given(@"I make the following manual merchant deposits")]
        public async Task GivenIMakeTheFollowingManualMerchantDeposits(DataTable table) {
            var estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails, Guid, MakeMerchantDepositRequest)> requests = table.Rows.ToMakeMerchantDepositRequest(estates);

            foreach ((EstateDetails, Guid, MakeMerchantDepositRequest) request in requests)
            {
                Decimal previousMerchantBalance = await this.GetMerchantBalance(request.Item2);

                await this.TransactionProcessorSteps.GivenIMakeTheFollowingManualMerchantDeposits(this.TestingContext.AccessToken, request);

                await Retry.For(async () => {
                                    Decimal currentMerchantBalance = await this.GetMerchantBalance(request.Item2);

                                    currentMerchantBalance.ShouldBe(previousMerchantBalance + request.Item3.Amount);

                                    this.TestingContext.Logger.LogInformation($"Deposit Reference {request.Item3.Reference} made for Merchant Id {request.Item2}");
                                });
            }
        }
        
        [Given(@"I have created the following security users")]
        public async Task GivenIHaveCreatedTheFollowingSecurityUsers(DataTable table)
        {
            var estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<CreateNewUserRequest> createUserRequests = table.Rows.ToCreateNewUserRequests(estates);
            await this.TransactionProcessorSteps.WhenICreateTheFollowingSecurityUsers(this.TestingContext.AccessToken, createUserRequests, estates);
        }

        [Given(@"I have assigned the following devices to the merchants")]
        public void GivenIHaveAssignedTheFollowingDevicesToTheMerchants(DataTable table) {
            

            
        }

        [When(@"I add the following contracts to the following merchants")]
        public async Task WhenIAddTheFollowingContractsToTheFollowingMerchants(DataTable table)
        {
            List<EstateDetails> estates = this.TestingContext.Estates.Select(e => e).ToList();
            List<(EstateDetails, Guid, Guid)> requests = table.Rows.ToAddContractToMerchantRequests(estates);
            await this.TransactionProcessorSteps.WhenIAddTheFollowingContractsToTheFollowingMerchants(this.TestingContext.AccessToken, requests);
        }

        [Given(@"the following bills are available at the PataPawa PostPaid Host")]
        public async Task GivenTheFollowingBillsAreAvailableAtThePataPawaPostPaidHost(DataTable table)
        {
            List<ReqnrollExtensions.PataPawaBill> bills = table.Rows.ToPataPawaBills();
            await this.TransactionProcessorSteps.GivenTheFollowingBillsAreAvailableAtThePataPawaPostPaidHost(bills);
        }

        [Given(@"the following users are available at the PataPawa PrePay Host")]
        public async Task GivenTheFollowingUsersAreAvailableAtThePataPawaPrePayHost(DataTable table)
        {
            List<ReqnrollExtensions.PataPawaUser> users = table.Rows.ToPataPawaUsers();
            await this.TransactionProcessorSteps.GivenTheFollowingUsersAreAvailableAtThePataPawaPrePaidHost(users);
        }

        [Given(@"the following meters are available at the PataPawa PrePay Host")]
        public async Task GivenTheFollowingMetersAreAvailableAtThePataPawaPrePayHost(DataTable table)
        {
            List<ReqnrollExtensions.PataPawaMeter> meters = table.Rows.ToPataPawaMeters();
            await this.TransactionProcessorSteps.GivenTheFollowingMetersAreAvailableAtThePataPawaPrePaidHost(meters);
        }
    }
}
