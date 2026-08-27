using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using Shouldly;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.Mobile.UiTestBackend;

namespace TransactionProcessor.Mobile.UITests.SmokeTests;

[TestFixture]
public class TestBackendHostSmokeTests
{
    [Test]
    public async Task Host_StartsAndHealthEndpointResponds()
    {
        await using TestBackendHost host = await TestBackendHost.StartAsync(GetFreePort()).ConfigureAwait(false);

        using HttpClient client = host.CreateClient();
        HttpResponseMessage response = await client.GetAsync("/health").ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        host.WindowsBaseUrl.ShouldStartWith("http://127.0.0.1:");
        host.AndroidBaseUrl.ShouldContain("10.0.2.2");
    }

    [Test]
    public async Task Seed_CanBeApplied_AndResetRestoresDefaultMerchantState()
    {
        await using TestBackendHost host = await TestBackendHost.StartAsync(GetFreePort()).ConfigureAwait(false);

        using HttpClient client = host.CreateClient();

        BackendSeed seed = new BackendSeed
        {
            Clients =
            [
                new ClientSeed
                {
                    ClientId = "mobileAppClient",
                    ClientName = "Mobile App",
                    Secret = "Secret1",
                    GrantTypes = ["client_credentials", "password"],
                    IsAppClient = true
                }
            ],
            Merchants =
            [
                new MerchantSeed
                {
                    EstateName = "Estate One",
                    MerchantName = "Seeded Merchant",
                    AddressLine1 = "1 Seed Street",
                    AddressLine2 = "Suite 1",
                    AddressLine3 = string.Empty,
                    AddressLine4 = string.Empty,
                    Town = "Town",
                    Region = "Region",
                    PostalCode = "SE1 1ED",
                    Country = "United Kingdom",
                    ContactName = "Seed Contact",
                    ContactEmailAddress = "seed@example.com",
                    ContactPhoneNumber = "123456789"
                }
            ]
        };

        HttpResponseMessage seedResponse = await client.PostAsJsonAsync("/api/test/seed", seed).ConfigureAwait(false);
        seedResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        MerchantResponse? seededMerchant = await client.GetFromJsonAsync<MerchantResponse>("/api/merchants").ConfigureAwait(false);
        seededMerchant.ShouldNotBeNull();
        seededMerchant.MerchantName.ShouldBe("Seeded Merchant");
        seededMerchant.Contacts.ShouldNotBeEmpty();
        seededMerchant.Contacts[0].ContactEmailAddress.ShouldBe("seed@example.com");

        HttpResponseMessage resetResponse = await client.PostAsync("/api/test/reset", null).ConfigureAwait(false);
        resetResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        MerchantResponse? resetMerchant = await client.GetFromJsonAsync<MerchantResponse>("/api/merchants").ConfigureAwait(false);
        resetMerchant.ShouldNotBeNull();
        resetMerchant.MerchantName.ShouldBe("Dummy Merchant");
        resetMerchant.Contacts.ShouldNotBeEmpty();
        resetMerchant.Contacts[0].ContactEmailAddress.ShouldBe("test@example.com");
    }

    [Test]
    public async Task TransactionMixSummary_ReturnsTransactionTypeAsString()
    {
        await using TestBackendHost host = await TestBackendHost.StartAsync(GetFreePort()).ConfigureAwait(false);

        using HttpClient client = host.CreateClient();
        SeedReportTransaction(host, RequestKind.MobileTopup);

        StringContent requestContent = new(
            JsonSerializer.Serialize(new
            {
                MerchantReportingId = 12345,
                StartDate = new DateTime(2026, 8, 27),
                EndDate = new DateTime(2026, 8, 27),
                Breakdown = 1,
                Measure = 1,
                TopN = 5
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower }),
            Encoding.UTF8,
            "application/json");

        HttpResponseMessage response = await client.PostAsync("/api/reporting/transactionmixsummary", requestContent).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        JsonElement transactionType = document.RootElement.GetProperty("drill_down_transactions")
                                                         .EnumerateArray()
                                                         .Single()
                                                         .GetProperty("transaction_type");

        transactionType.ValueKind.ShouldBe(JsonValueKind.String);
        transactionType.GetString().ShouldBe("Mobile Topup");
    }

    [Test]
    public async Task RecentActivityReceiptSearch_ReturnsTransactionTypeAsString()
    {
        await using TestBackendHost host = await TestBackendHost.StartAsync(GetFreePort()).ConfigureAwait(false);

        using HttpClient client = host.CreateClient();
        SeedReportTransaction(host, RequestKind.BillPaymentMakePayment);

        StringContent requestContent = new(
            JsonSerializer.Serialize(new
            {
                MerchantReportingId = 12345,
                ReportDate = new DateTime(2026, 8, 27),
                SearchText = (string?)null,
                PageNumber = 1,
                PageSize = 5
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower }),
            Encoding.UTF8,
            "application/json");

        HttpResponseMessage response = await client.PostAsync("/api/reporting/recentactivityreceiptsearch", requestContent).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        JsonElement transactionType = document.RootElement.GetProperty("items")
                                                         .EnumerateArray()
                                                         .Single()
                                                         .GetProperty("transaction_type");

        transactionType.ValueKind.ShouldBe(JsonValueKind.String);
        transactionType.GetString().ShouldBe("Bill Payment");
    }

    private static int GetFreePort()
    {
        using TcpListener listener = new(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        return port;
    }

    private static void SeedReportTransaction(TestBackendHost host, RequestKind transactionType)
    {
        BackendSeed seed = new()
        {
            ReportTransactions =
            [
                new ReportTransactionSeed
                {
                    Reference = "TXN-10001",
                    ReceiptReference = "RCPT-10001",
                    TransactionType = transactionType,
                    Product = "Custom",
                    Operator = "Safaricom",
                    Status = "Success",
                    Amount = 100.00m,
                    TransactionDateTime = new DateTime(2026, 8, 27, 9, 30, 0)
                }
            ]
        };

        host.ApplySeed(seed);
    }
}
