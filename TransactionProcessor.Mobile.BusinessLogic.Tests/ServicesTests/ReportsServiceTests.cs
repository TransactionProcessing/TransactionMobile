using Moq;
using RichardSzalay.MockHttp;
using Shouldly;
using SimpleResults;
using System.Net;
using System.Text;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ServicesTests;

public class ReportsServiceTests
{
    private readonly MockHttpMessageHandler MockHttpMessageHandler;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IApplicationInfoService> ApplicationInfoService;
    private readonly IReportsService ReportsService;

    public ReportsServiceTests()
    {
        this.MockHttpMessageHandler = new MockHttpMessageHandler();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.ApplicationInfoService = new Mock<IApplicationInfoService>();
        this.ApplicationCache.Setup(s => s.GetAccessToken()).Returns(new TokenResponseModel
        {
            AccessToken = "token"
        });
        this.ApplicationInfoService.Setup(s => s.VersionString).Returns("1.2.3");
        this.ReportsService = new ReportsService(_ => "http://localhost",
                                                 this.MockHttpMessageHandler.ToHttpClient(),
                                                 this.ApplicationCache.Object,
                                                 obj => StringSerialiser.Serialise(obj),
                                                 (json, type) => StringSerialiser.DeserializeObject<object>(json, type),
                                                 this.ApplicationInfoService.Object);
        Logger.Initialise(new NullLogger());
        StringSerialiser.Initialise((IStringSerialiser)new SystemTextJsonSerializer(SystemTextJsonSerializer.GetDefaultJsonSerializerOptions()));
    }

    [Fact]
    public async Task GetTransactionMixSummary_SendsRequestAndMapsResponse()
    {
        MerchantTransactionMixSummaryResponseDto response = new()
        {
            FromDate = new DateTime(2026, 7, 1),
            ToDate = new DateTime(2026, 7, 31),
            Breakdown = TransactionMixBreakdown.Product,
            Measure = TransactionMixMeasure.Value,
            TotalCount = 3,
            TotalValue = 3110.00m,
            Items =
            [
                new TransactionMixSummaryItemDto { Key = "custom", Label = "Custom", Count = 2, Value = 1250.00m },
                new TransactionMixSummaryItemDto { Key = "bill-pay-post", Label = "Bill Pay (Post)", Count = 1, Value = 1900.00m },
            ],
            DrillDownTransactions =
            [
                new TransactionMixDrillDownTransactionDto
                {
                    Reference = "TXN-10001",
                    TransactionType = "Mobile Topup",
                    Product = "Custom",
                    Operator = "Safaricom",
                    Status = "Success",
                    Amount = 100.00m,
                    TransactionDateTime = new DateTime(2026, 7, 6, 9, 30, 0)
                }
            ]
        };

        String requestPayload = string.Empty;
        this.MockHttpMessageHandler.When(HttpMethod.Post, "http://localhost/api/reporting/transactionmixsummary?applicationVersion=1.2.3")
            .Respond(req =>
            {
                requestPayload = req.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(StringSerialiser.Serialise(response), Encoding.UTF8, "application/json")
                };
            });

        Result<TransactionMixSummaryModel> result = await this.ReportsService.GetTransactionMixSummary(12345,
                                                                                                       new DateTime(2026, 7, 1),
                                                                                                       new DateTime(2026, 7, 31),
                                                                                                       TransactionMixBreakdown.Product,
                                                                                                       TransactionMixMeasure.Value,
                                                                                                       5,
                                                                                                       CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Breakdown.ShouldBe(TransactionMixBreakdown.Product);
        result.Data.Measure.ShouldBe(TransactionMixMeasure.Value);
        result.Data.TotalCount.ShouldBe(3);
        result.Data.TotalValue.ShouldBe(3110.00m);
        result.Data.Items.Count.ShouldBe(2);
        result.Data.DrillDownTransactions.Count.ShouldBe(1);
        requestPayload.ShouldContain("\"merchant_reporting_id\": 12345");
        requestPayload.ShouldContain("\"top_n\": 5");
    }
}
