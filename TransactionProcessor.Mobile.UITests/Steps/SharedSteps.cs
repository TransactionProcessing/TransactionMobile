using Reqnroll;
using SecurityService.DataTransferObjects;
using Shared.IntegrationTesting;
using Shouldly;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TransactionProcessor.IntegrationTesting.Helpers;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.Mobile.UiTestBackend;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps;

using ClientDetails = Common.ClientDetails;
using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;

[Binding]
[Scope(Tag = "shared")]
public class SharedSteps
{
    private readonly TestingContext testingContext;
    private readonly LoginPage loginPage;

    public SharedSteps(TestingContext testingContext)
    {
        this.testingContext = testingContext;
        this.loginPage = new LoginPage(testingContext);
    }

    [Given(@"the following security roles exist")]
    public Task GivenTheFollowingSecurityRolesExist(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string roleName = GetString(row, "Role Name", "Name");
            this.testingContext.Roles[roleName] = roleName;
        }

        return Task.CompletedTask;
    }

    [Given(@"I create the following api scopes")]
    public Task GivenICreateTheFollowingApiScopes(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string name = GetString(row, "Name");
            this.testingContext.ApiResources.Add(name);
        }

        return Task.CompletedTask;
    }

    [Given(@"the following api resources exist")]
    public Task GivenTheFollowingApiResourcesExist(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string name = GetString(row, "Name");
            if (this.testingContext.ApiResources.Contains(name) == false)
            {
                this.testingContext.ApiResources.Add(name);
            }
        }

        return Task.CompletedTask;
    }

    [Given(@"the following clients exist")]
    public Task GivenTheFollowingClientsExist(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string clientId = GetString(row, "ClientId");
            string clientName = GetString(row, "ClientName");
            string secret = GetString(row, "Secret");
            List<string> grantTypes = GetString(row, "GrantTypes").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            this.testingContext.AddClientDetails(clientId, secret, grantTypes);
            this.testingContext.ScenarioSeed.Clients.Add(new ClientSeed
            {
                ClientId = clientId,
                ClientName = clientName,
                Secret = secret,
                GrantTypes = grantTypes,
                IsAppClient = grantTypes.Any(g => string.Equals(g, "password", StringComparison.OrdinalIgnoreCase))
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I have a token to access the estate management and transaction processor acl resources")]
    public async Task GivenIHaveATokenToAccessTheEstateManagementAndTransactionProcessorAclResources(DataTable table)
    {
        DataTableRow firstRow = table.Rows.First();
        string clientId = GetString(firstRow, "ClientId").Replace("[id]", this.testingContext.TestHostHelper.TestId.ToString("N"));
        ClientDetails clientDetails = this.testingContext.GetClientDetails(clientId);

        this.testingContext.AccessToken = await this.RequestAccessTokenAsync(clientDetails.ClientId, clientDetails.ClientSecret).ConfigureAwait(false);
    }

    [Given(@"I have created the following estates")]
    public Task GivenIHaveCreatedTheFollowingEstates(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string estateName = GetString(row, "EstateName");
            string estateReference = GetOptionalString(row, "EstateReference", "Reference") ?? estateName;
            Guid estateId = CreateGuid($"estate:{estateName}");

            EnsureEstate(estateId, estateName, estateReference);
            this.testingContext.ScenarioSeed.Estates.Add(new EstateSeed
            {
                EstateName = estateName,
                EstateReference = estateReference
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I have created the following operators")]
    public Task GivenIHaveCreatedTheFollowingOperators(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string estateName = GetString(row, "EstateName");
            string operatorName = GetString(row, "OperatorName");
            bool requireCustomMerchantNumber = GetBoolean(row, "RequireCustomMerchantNumber");
            bool requireCustomTerminalNumber = GetBoolean(row, "RequireCustomTerminalNumber");

            this.testingContext.ScenarioSeed.Operators.Add(new OperatorSeed
            {
                EstateName = estateName,
                OperatorName = operatorName,
                RequireCustomMerchantNumber = requireCustomMerchantNumber,
                RequireCustomTerminalNumber = requireCustomTerminalNumber
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given("I have assigned the following operators to the estates")]
    public Task GivenIHaveAssignedTheFollowingOperatorsToTheEstates(DataTable dataTable)
    {
        foreach (DataTableRow row in dataTable.Rows)
        {
            this.testingContext.ScenarioSeed.Operators.Add(new OperatorSeed
            {
                EstateName = GetString(row, "EstateName"),
                OperatorName = GetString(row, "OperatorName")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I create a contract with the following values")]
    public Task GivenICreateAContractWithTheFollowingValues(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Contracts.Add(new ContractSeed
            {
                EstateName = GetString(row, "EstateName"),
                OperatorName = GetString(row, "OperatorName"),
                ContractDescription = GetString(row, "ContractDescription")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [When(@"I create the following Products")]
    public Task WhenICreateTheFollowingProducts(Table table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Products.Add(new ProductSeed
            {
                EstateName = GetString(row, "EstateName"),
                OperatorName = GetString(row, "OperatorName"),
                ContractDescription = GetString(row, "ContractDescription"),
                ProductName = GetString(row, "ProductName"),
                DisplayText = GetString(row, "DisplayText"),
                ProductType = GetString(row, "ProductType"),
                Value = GetOptionalDecimal(row, "Value")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [When(@"I add the following Transaction Fees")]
    public Task WhenIAddTheFollowingTransactionFees(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            int index = this.testingContext.ScenarioSeed.Products.FindIndex(product =>
                string.Equals(product.EstateName, GetString(row, "EstateName"), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(product.OperatorName, GetString(row, "OperatorName"), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(product.ContractDescription, GetString(row, "ContractDescription"), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(product.ProductName, GetString(row, "ProductName"), StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                ProductSeed product = this.testingContext.ScenarioSeed.Products[index];
                this.testingContext.ScenarioSeed.Products[index] = product with
                {
                    CalculationType = GetString(row, "CalculationType"),
                    FeeDescription = GetString(row, "FeeDescription"),
                    FeeValue = GetOptionalDecimal(row, "Value")
                };
            }
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I create the following merchants")]
    public Task GivenICreateTheFollowingMerchants(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string estateName = GetString(row, "EstateName");
            string estateReference = GetOptionalString(row, "EstateReference") ?? estateName;
            string merchantName = GetString(row, "MerchantName");
            string emailAddress = GetOptionalString(row, "EmailAddress", "ContactEmailAddress") ?? $"{merchantName.Replace(" ", string.Empty).ToLowerInvariant()}@example.com";
            string contactName = GetOptionalString(row, "ContactName", "MerchantContactName") ?? merchantName;

            this.testingContext.ScenarioSeed.Merchants.Add(new MerchantSeed
            {
                EstateName = estateName,
                MerchantName = merchantName,
                AddressLine1 = GetString(row, "AddressLine1"),
                AddressLine2 = GetString(row, "AddressLine2"),
                AddressLine3 = GetString(row, "AddressLine3"),
                AddressLine4 = GetString(row, "AddressLine4"),
                Town = GetString(row, "Town"),
                Region = GetString(row, "Region"),
                PostalCode = GetString(row, "PostalCode"),
                Country = GetString(row, "Country"),
                ContactName = contactName,
                ContactEmailAddress = emailAddress,
                ContactPhoneNumber = GetOptionalString(row, "ContactPhoneNumber", "PhoneNumber") ?? "123456789",
                SettlementSchedule = GetOptionalString(row, "SettlementSchedule")
            });

            EstateDetails estate = EnsureEstate(CreateGuid($"estate:{estateName}"), estateName, estateReference);
            MerchantResponse merchantResponse = CreateMerchantResponse(estateName, merchantName, row);
            estate.AddMerchant(merchantResponse);
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I have assigned the following  operator to the merchants")]
    public Task GivenIHaveAssignedTheFollowingOperatorToTheMerchants(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.MerchantOperators.Add(new MerchantOperatorSeed
            {
                EstateName = GetString(row, "EstateName"),
                MerchantName = GetString(row, "MerchantName"),
                OperatorName = GetString(row, "OperatorName"),
                MerchantNumber = GetString(row, "MerchantNumber"),
                TerminalNumber = GetString(row, "TerminalNumber")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I have assigned the following devices to the merchants")]
    public Task GivenIHaveAssignedTheFollowingDevicesToTheMerchants(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Devices.Add(new DeviceSeed
            {
                EstateName = GetString(row, "EstateName"),
                MerchantName = GetString(row, "MerchantName")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [When(@"I add the following contracts to the following merchants")]
    public Task WhenIAddTheFollowingContractsToTheFollowingMerchants(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.MerchantContracts.Add(new MerchantContractSeed
            {
                EstateName = GetString(row, "EstateName"),
                MerchantName = GetString(row, "MerchantName"),
                ContractDescription = GetString(row, "ContractDescription")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I have created the following security users")]
    public Task GivenIHaveCreatedTheFollowingSecurityUsers(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            string userName = GetOptionalString(row, "EmailAddress", "Username", "UserName") ?? string.Empty;
            this.testingContext.ScenarioSeed.Users.Add(new UserSeed
            {
                UserName = userName,
                Password = GetString(row, "Password"),
                GivenName = GetOptionalString(row, "GivenName"),
                FamilyName = GetOptionalString(row, "FamilyName")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I have created a config for my device")]
    public async Task GivenIHaveCreatedAConfigForMyDevice()
    {
        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        this.testingContext.Logger.LogInformation("Config seeded for the current device. The backend will bind the device when the app requests configuration.");
    }

    [Given(@"I have created a config for my application")]
    public Task GivenIHaveCreatedAConfigForMyApplication()
    {
        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"I make the following manual merchant deposits")]
    public Task GivenIMakeTheFollowingManualMerchantDeposits(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Deposits.Add(new DepositSeed
            {
                EstateName = GetString(row, "EstateName"),
                MerchantName = GetString(row, "MerchantName"),
                Reference = GetString(row, "Reference"),
                Amount = GetDecimal(row, "Amount"),
                DateTime = GetDateTime(row, "DateTime")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"the following transaction mix report transactions exist")]
    public Task GivenTheFollowingTransactionMixReportTransactionsExist(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.ReportTransactions.Add(new ReportTransactionSeed
            {
                Reference = GetString(row, "Reference"),
                TransactionType = GetString(row, "TransactionType"),
                Product = GetString(row, "Product"),
                Operator = GetString(row, "Operator"),
                Status = GetString(row, "Status"),
                Amount = GetDecimal(row, "Amount"),
                TransactionDateTime = GetDateTime(row, "TransactionDateTime"),
                ReceiptReference = GetOptionalString(row, "ReceiptReference") ?? $"RCPT-{GetString(row, "Reference")}"
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"the following bills are available at the PataPawa PostPaid Host")]
    public Task GivenTheFollowingBillsAreAvailableAtThePataPawaPostPaidHost(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Bills.Add(new BillSeed
            {
                AccountNumber = GetString(row, "AccountNumber"),
                AccountName = GetString(row, "AccountName"),
                DueDate = GetDateTime(row, "DueDate"),
                Amount = GetDecimal(row, "Amount")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"the following users are available at the PataPawa PrePay Host")]
    public Task GivenTheFollowingUsersAreAvailableAtThePataPawaPrePayHost(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Users.Add(new UserSeed
            {
                UserName = GetString(row, "Username"),
                Password = GetString(row, "Password")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"the following meters are available at the PataPawa PrePay Host")]
    public Task GivenTheFollowingMetersAreAvailableAtThePataPawaPrePayHost(DataTable table)
    {
        foreach (DataTableRow row in table.Rows)
        {
            this.testingContext.ScenarioSeed.Meters.Add(new MeterSeed
            {
                MeterNumber = GetString(row, "MeterNumber"),
                CustomerName = GetString(row, "CustomerName")
            });
        }

        this.testingContext.TestHostHelper.ApplySeed(this.testingContext.ScenarioSeed);
        return Task.CompletedTask;
    }

    [Given(@"the application is in training mode")]
    public async Task GivenTheApplicationIsInTrainingMode()
    {
        bool isTrainingModeOn = await this.loginPage.IsTrainingModeOn().ConfigureAwait(false);
        if (isTrainingModeOn == false)
        {
            await this.loginPage.SetTrainingModeOn().ConfigureAwait(false);
        }
    }

    private static string GetString(DataTableRow row, params string[] columns)
    {
        foreach (string column in columns)
        {
            if (row.ContainsKey(column))
            {
                return ReqnrollTableHelper.GetStringRowValue(row, column);
            }
        }

        throw new KeyNotFoundException($"Missing expected column: {string.Join(", ", columns)}");
    }

    private static string? GetOptionalString(DataTableRow row, params string[] columns)
    {
        foreach (string column in columns)
        {
            if (row.ContainsKey(column))
            {
                string value = ReqnrollTableHelper.GetStringRowValue(row, column);
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    return value;
                }
            }
        }

        return null;
    }

    private static bool GetBoolean(DataTableRow row, string column)
    {
        string value = GetString(row, column);
        return bool.TryParse(value, out bool parsed) && parsed;
    }

    private static decimal GetDecimal(DataTableRow row, string column)
    {
        string value = GetString(row, column);
        return decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static decimal? GetOptionalDecimal(DataTableRow row, string column)
    {
        string? value = GetOptionalString(row, column);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static DateTime GetDateTime(DataTableRow row, string column)
    {
        string value = GetString(row, column);
        if (string.Equals(value, "Today", StringComparison.OrdinalIgnoreCase))
        {
            return DateTime.Today;
        }

        return DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
    }

    private EstateDetails EnsureEstate(Guid estateId, string estateName, string estateReference)
    {
        EstateDetails estate = this.testingContext.Estates.FirstOrDefault(e => e.EstateId == estateId || string.Equals(e.EstateName, estateName, StringComparison.OrdinalIgnoreCase));
        if (estate != null)
        {
            return estate;
        }

        this.testingContext.AddEstateDetails(estateId, estateName, estateReference);
        return this.testingContext.GetEstateDetails(estateId);
    }

    private MerchantResponse CreateMerchantResponse(string estateName, string merchantName, DataTableRow row)
    {
        string emailAddress = GetOptionalString(row, "EmailAddress", "ContactEmailAddress") ?? $"{merchantName.Replace(" ", string.Empty).ToLowerInvariant()}@example.com";
        string contactName = GetOptionalString(row, "ContactName", "MerchantContactName") ?? merchantName;

        return new MerchantResponse
        {
            EstateId = CreateGuid($"estate:{estateName}"),
            EstateReportingId = CreateInt($"estate-report:{estateName}"),
            MerchantId = CreateGuid($"merchant:{estateName}:{merchantName}"),
            MerchantReportingId = CreateInt($"merchant-report:{estateName}:{merchantName}"),
            MerchantName = merchantName,
            MerchantReference = merchantName,
            NextStatementDate = DateTime.Today.AddDays(30),
            SettlementSchedule = SettlementSchedule.Monthly,
            Addresses =
            [
                new AddressResponse
                {
                    AddressId = CreateGuid($"address:{estateName}:{merchantName}"),
                    AddressLine1 = GetString(row, "AddressLine1"),
                    AddressLine2 = GetString(row, "AddressLine2"),
                    AddressLine3 = GetString(row, "AddressLine3"),
                    AddressLine4 = GetString(row, "AddressLine4"),
                    Town = GetString(row, "Town"),
                    Region = GetString(row, "Region"),
                    PostalCode = GetString(row, "PostalCode"),
                    Country = GetString(row, "Country")
                }
            ],
            Contacts =
            [
                new ContactResponse
                {
                    ContactId = CreateGuid($"contact:{estateName}:{merchantName}"),
                    ContactName = contactName,
                    ContactEmailAddress = emailAddress,
                    ContactPhoneNumber = GetOptionalString(row, "ContactPhoneNumber", "PhoneNumber") ?? "123456789"
                }
            ],
            Devices = new Dictionary<Guid, string>(),
            Operators = [],
            Contracts = [],
            OpeningHours = new Dictionary<DayOfWeek, OpeningHoursResponse>()
        };
    }

    private async Task<string> RequestAccessTokenAsync(string clientId, string clientSecret)
    {
        HttpClient client = this.testingContext.BackendHost?.CreateClient()
                             ?? this.testingContext.TestHostHelper.TestHostHttpClient
                             ?? throw new InvalidOperationException("Backend host is not available.");

        using HttpRequestMessage request = new(HttpMethod.Post, "/connect/token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret
        });

        using HttpResponseMessage response = await client.SendAsync(request, CancellationToken.None).ConfigureAwait(false);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        TokenResponsePayload tokenResponse = DeserializeTokenResponse(content);
        tokenResponse.AccessToken.ShouldNotBeNullOrWhiteSpace();
        return tokenResponse.AccessToken;
    }

    private static TokenResponsePayload DeserializeTokenResponse(string content)
    {
        using JsonDocument document = JsonDocument.Parse(content);
        JsonElement root = document.RootElement;

        string accessToken =
            TryGetString(root, "accessToken")
            ?? TryGetString(root, "access_token")
            ?? throw new InvalidOperationException($"Token response did not contain an access token. Payload: {content}");

        string? refreshToken =
            TryGetString(root, "refreshToken")
            ?? TryGetString(root, "refresh_token");

        long expiresIn =
            TryGetInt64(root, "expiresIn")
            ?? TryGetInt64(root, "expires_in")
            ?? 0;

        return new TokenResponsePayload(accessToken, refreshToken, expiresIn);
    }

    private static string? TryGetString(JsonElement element, string propertyName)
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(propertyName, out JsonElement property) &&
            property.ValueKind == JsonValueKind.String)
        {
            return property.GetString();
        }

        return null;
    }

    private static long? TryGetInt64(JsonElement element, string propertyName)
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(propertyName, out JsonElement property) &&
            property.TryGetInt64(out long value))
        {
            return value;
        }

        return null;
    }

    private static Guid CreateGuid(string value)
    {
        byte[] hash = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(value));
        Span<byte> guidBytes = stackalloc byte[16];
        hash.AsSpan(0, 16).CopyTo(guidBytes);
        return new Guid(guidBytes);
    }

    private static int CreateInt(string value)
    {
        byte[] hash = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(value.ToUpperInvariant()));
        int valueHash = BitConverter.ToInt32(hash, 0);
        return Math.Abs(valueHash == int.MinValue ? int.MaxValue : valueHash);
    }
}

internal sealed record TokenResponsePayload(string AccessToken, string? RefreshToken, long ExpiresIn);

