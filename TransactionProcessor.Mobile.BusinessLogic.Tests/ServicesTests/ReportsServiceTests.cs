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

    [Fact]
    public async Task GetRecentActivityReceiptReport_SendsRequestAndMapsResponse()
    {
        RecentActivityReceiptSearchResponseDto response = new()
        {
            ReportDate = new DateTime(2026, 7, 6),
            PageNumber = 1,
            PageSize = 5,
            TotalCount = 2,
            Items =
            [
                new RecentActivityReceiptSearchItemDto
                {
                    Reference = "TXN-10002",
                    TransactionType = "Bill Payment",
                    Product = "Bill Pay (Post)",
                    Operator = "PataPawa PostPay",
                    Status = "Success",
                    Amount = 250.00m,
                    TransactionDateTime = new DateTime(2026, 7, 6, 10, 15, 0),
                    ReceiptReference = "RCPT-10002"
                },
                new RecentActivityReceiptSearchItemDto
                {
                    Reference = "TXN-10001",
                    TransactionType = "Mobile Topup",
                    Product = "Custom",
                    Operator = "Safaricom",
                    Status = "Success",
                    Amount = 100.00m,
                    TransactionDateTime = new DateTime(2026, 7, 6, 9, 30, 0),
                    ReceiptReference = "RCPT-10001"
                }
            ]
        };

        String requestPayload = string.Empty;
        this.MockHttpMessageHandler.When(HttpMethod.Post, "http://localhost/api/reporting/recentactivityreceiptsearch")
            .Respond(req =>
            {
                requestPayload = req.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(StringSerialiser.Serialise(response), Encoding.UTF8, "application/json")
                };
            });

        Result<RecentActivityReceiptReportModel> result = await this.ReportsService.GetRecentActivityReceiptReport(12345,
                                                                                                              new DateTime(2026, 7, 6),
                                                                                                              "TXN-10001",
                                                                                                              1,
                                                                                                              5,
                                                                                                              CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ReportDate.ShouldBe(new DateTime(2026, 7, 6));
        result.Data.Items.Count.ShouldBe(2);
        result.Data.Items[0].Reference.ShouldBe("TXN-10002");
        result.Data.Items[1].Reference.ShouldBe("TXN-10001");
        result.Data.PageNumber.ShouldBe(1);
        result.Data.PageSize.ShouldBe(5);
        result.Data.TotalCount.ShouldBe(2);
        requestPayload.ShouldContain("\"application_version\": \"1.2.3\"");
        requestPayload.ShouldContain("\"merchant_reporting_id\": 12345");
        requestPayload.ShouldContain("\"report_date\"");
        requestPayload.ShouldContain("2026-07-06");
        requestPayload.ShouldContain("\"search_text\": \"TXN-10001\"");
    }

    [Fact]
    public async Task GetRecentActivityReceiptReport_ReturnsSecondPage()
    {
        RecentActivityReceiptSearchResponseDto response = new()
        {
            ReportDate = new DateTime(2026, 7, 6),
            PageNumber = 2,
            PageSize = 5,
            TotalCount = 10,
            Items =
            [
                new RecentActivityReceiptSearchItemDto
                {
                    Reference = "TXN-10005",
                    TransactionType = "Bill Payment",
                    Product = "Prepaid Power",
                    Operator = "KPLC",
                    Status = "Failed",
                    Amount = 75.00m,
                    TransactionDateTime = new DateTime(2026, 7, 6, 13, 5, 0),
                    ReceiptReference = "RCPT-10005"
                }
            ]
        };

        this.MockHttpMessageHandler.When(HttpMethod.Post, "http://localhost/api/reporting/recentactivityreceiptsearch")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(StringSerialiser.Serialise(response), Encoding.UTF8, "application/json")
            });

        Result<RecentActivityReceiptReportModel> result = await this.ReportsService.GetRecentActivityReceiptReport(12345,
                                                                                                              new DateTime(2026, 7, 6),
                                                                                                              null,
                                                                                                              2,
                                                                                                              5,
                                                                                                              CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.PageNumber.ShouldBe(2);
        result.Data.PageSize.ShouldBe(5);
        result.Data.TotalCount.ShouldBe(10);
        result.Data.Items.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ResendRecentActivityReceipt_SendsRequestAndMapsSuccessResponse()
    {
        RecentActivityReceiptResendResponseDto response = new()
        {
            Success = true,
            Message = "Receipt resend requested.",
            Reference = "TXN-10001",
            ReceiptReference = "RCPT-10001",
            TransactionReference = "TXN-10001"
        };

        String requestPayload = string.Empty;
        this.MockHttpMessageHandler.When(HttpMethod.Post, "http://localhost/api/transactions/resendreceipt")
            .Respond(req =>
            {
                requestPayload = req.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(StringSerialiser.Serialise(response), Encoding.UTF8, "application/json")
                };
            });

        Result<RecentActivityReceiptResendResultModel> result = await this.ReportsService.ResendRecentActivityReceipt("TXN-10001",
                                                                                                                      "customer@example.com",
                                                                                                                      CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Success.ShouldBeTrue();
        result.Data.Message.ShouldBe("Receipt resend requested.");
        result.Data.Reference.ShouldBe("TXN-10001");
        requestPayload.ShouldContain("\"reference\": \"TXN-10001\"");
        requestPayload.ShouldContain("\"recipient_email_address\": \"customer@example.com\"");
    }

    [Fact]
    public async Task ResendRecentActivityReceipt_ReturnsFailureWhenApiRejectsRequest()
    {
        RecentActivityReceiptResendResponseDto response = new()
        {
            Success = false,
            Message = "Recipient email address is not valid."
        };

        this.MockHttpMessageHandler.When(HttpMethod.Post, "http://localhost/api/transactions/resendreceipt")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(StringSerialiser.Serialise(response), Encoding.UTF8, "application/json")
            });

        Result<RecentActivityReceiptResendResultModel> result = await this.ReportsService.ResendRecentActivityReceipt("TXN-10001",
                                                                                                                      "invalid@example",
                                                                                                                      CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Message.ShouldBe("Recipient email address is not valid.");
    }
}
