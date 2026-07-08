using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessorACL.DataTransferObjects.Responses;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface IReportsService {
        Task<Result<DailyPerformanceSummaryModel>> GetDailyPerformanceSummary(PerformanceSummaryPeriod period, int merchantReportingId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<Result<TransactionMixSummaryModel>> GetTransactionMixSummary(int merchantReportingId,
                                                                          DateTime startDate,
                                                                          DateTime endDate,
                                                                          TransactionMixBreakdown breakdown,
                                                                          TransactionMixMeasure measure,
                                                                          int topN,
                                                                          CancellationToken cancellationToken);
        Task<Result<RecentActivityReceiptReportModel>> GetRecentActivityReceiptReport(int merchantReportingId,
                                                                                      DateTime reportDate,
                                                                                      string? searchText,
                                                                                      int pageNumber,
                                                                                      int pageSize,
                                                                                      CancellationToken cancellationToken);

        Task<Result<RecentActivityReceiptResendResultModel>> ResendRecentActivityReceipt(string reference,
                                                                                          string recipientEmailAddress,
                                                                                          CancellationToken cancellationToken);
    }
    public class ReportsService : ClientProxyBase.ClientProxyBase, IReportsService
    {
        #region Fields

        private readonly IApplicationCache ApplicationCache;
        private readonly IApplicationInfoService ApplicationInfoService;

        private readonly Func<String, String> BaseAddressResolver;

        #endregion
        public ReportsService(Func<String, String> baseAddressResolver,
                              HttpClient httpClient,
                              IApplicationCache applicationCache, Func<Object, String> serialise,
                              Func<String, Type, Object> deserialise,
                              IApplicationInfoService applicationInfoService) : base(httpClient, serialise, deserialise)
        {
            this.BaseAddressResolver = baseAddressResolver;
            this.ApplicationCache = applicationCache;
            this.ApplicationInfoService = applicationInfoService;
        }

        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        public async Task<Result<DailyPerformanceSummaryModel>> GetDailyPerformanceSummary(PerformanceSummaryPeriod period,int merchantReportingId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

            String requestUri = this.BuildRequestUrl($"/api/reporting/dailymerchantprformancesummary?applicationVersion={this.ApplicationInfoService.VersionString}");

            MerchantDailyPerformanceSummaryRequest requestBody = new (){ MerchantReportingId = merchantReportingId, StartDate = startDate, EndDate = endDate };
            
            Result<MerchantDailyPerformanceSummaryResponse>? responseDataResult = await this.Post<MerchantDailyPerformanceSummaryRequest, MerchantDailyPerformanceSummaryResponse>(requestUri, requestBody, accessToken.AccessToken, cancellationToken);

            if (responseDataResult.IsFailed)
            {
                Logger.LogInformation($"GetDailyPerformanceSummary failed {responseDataResult.Status}");
                return ResultHelpers.CreateFailure(responseDataResult);
            }
            
            DailyPerformanceSummaryModel model = new() { DrillDownTransactions = responseDataResult.Data.DrillDownTransactions.Select(d => new DailyPerformanceTransactionModel(d.Reference, d.Product, d.Status, d.Amount, d.TransactionDateTime)
                ).ToList(),
                Metrics = responseDataResult.Data.Metrics.Select(m => new DailyPerformanceMetricModel(
                    m.Title,
                    m.Value.ToString("N2"),
                    m.Description,
                    (DailyPerformanceMetricCategory)m.Category
                )).ToList(),
                Period = period,
                PeriodLabel = period.ToString(),
                ToDate = endDate,
                FromDate = startDate
            };

            return Result.Success(model);

        }

        public async Task<Result<TransactionMixSummaryModel>> GetTransactionMixSummary(int merchantReportingId,
                                                                                       DateTime startDate,
                                                                                       DateTime endDate,
                                                                                       TransactionMixBreakdown breakdown,
                                                                                       TransactionMixMeasure measure,
                                                                                       int topN,
                                                                                       CancellationToken cancellationToken)
        {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

            String requestUri = this.BuildRequestUrl($"/api/reporting/transactionmixsummary?applicationVersion={this.ApplicationInfoService.VersionString}");

            MerchantTransactionMixSummaryRequest requestBody = new()
            {
                MerchantReportingId = merchantReportingId,
                StartDate = startDate,
                EndDate = endDate,
                Breakdown = breakdown,
                Measure = measure,
                TopN = topN
            };

            Result<MerchantTransactionMixSummaryResponseDto>? responseDataResult = await this.Post<MerchantTransactionMixSummaryRequest, MerchantTransactionMixSummaryResponseDto>(requestUri, requestBody, accessToken.AccessToken, cancellationToken);

            if (responseDataResult.IsFailed)
            {
                Logger.LogInformation($"GetTransactionMixSummary failed {responseDataResult.Status}");
                return ResultHelpers.CreateFailure(responseDataResult);
            }

            TransactionMixSummaryModel model = new()
            {
                FromDate = responseDataResult.Data.FromDate,
                ToDate = responseDataResult.Data.ToDate,
                Breakdown = responseDataResult.Data.Breakdown,
                Measure = responseDataResult.Data.Measure,
                TotalCount = responseDataResult.Data.TotalCount,
                TotalValue = responseDataResult.Data.TotalValue,
                Items = responseDataResult.Data.Items.Select(i => new TransactionMixSummaryItemModel(i.Key, i.Label, i.Count, i.Value)).ToList(),
                DrillDownTransactions = responseDataResult.Data.DrillDownTransactions.Select(t => new TransactionMixDrillDownTransactionModel(t.Reference,
                                                                                                                                        t.TransactionType,
                                                                                                                                        t.Product,
                                                                                                                                        t.Operator,
                                                                                                                                        t.Status,
                                                                                                                                        t.Amount,
                                                                                                                                        t.TransactionDateTime)).ToList()
            };

            return Result.Success(model);
        }

        public async Task<Result<RecentActivityReceiptReportModel>> GetRecentActivityReceiptReport(int merchantReportingId,
                                                                                                   DateTime reportDate,
                                                                                                   string? searchText,
                                                                                                   int pageNumber,
                                                                                                   int pageSize,
                                                                                                   CancellationToken cancellationToken)
        {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

            String requestUri = this.BuildRequestUrl("/api/reporting/recentactivityreceiptsearch");

            RecentActivityReceiptSearchRequest requestBody = new()
            {
                ApplicationVersion = this.ApplicationInfoService.VersionString,
                MerchantReportingId = merchantReportingId,
                ReportDate = reportDate.Date,
                SearchText = string.IsNullOrWhiteSpace(searchText) ? null : searchText.Trim(),
                PageNumber = pageNumber > 0 ? pageNumber : 1,
                PageSize = pageSize > 0 ? pageSize : 5
            };

            Result<RecentActivityReceiptSearchResponseDto>? responseDataResult = await this.Post<RecentActivityReceiptSearchRequest, RecentActivityReceiptSearchResponseDto>(requestUri, requestBody, accessToken.AccessToken, cancellationToken);

            if (responseDataResult.IsFailed)
            {
                Logger.LogInformation($"GetRecentActivityReceiptReport failed {responseDataResult.Status}");
                return ResultHelpers.CreateFailure(responseDataResult);
            }

            RecentActivityReceiptReportModel model = new()
            {
                ReportDate = responseDataResult.Data.ReportDate,
                SearchText = requestBody.SearchText,
                PageNumber = responseDataResult.Data.PageNumber,
                PageSize = responseDataResult.Data.PageSize,
                TotalCount = responseDataResult.Data.TotalCount,
                Items = responseDataResult.Data.Items.Select(item => new RecentActivityReceiptItemModel(item.Reference,
                                                                                                      item.TransactionType,
                                                                                                      item.Product,
                                                                                                      item.Operator,
                                                                                                      item.Status,
                                                                                                      item.Amount,
                                                                                                      item.TransactionDateTime,
                                                                                                      item.ReceiptReference)).ToList()
            };

            return Result.Success(model);
        }

        public async Task<Result<RecentActivityReceiptResendResultModel>> ResendRecentActivityReceipt(string reference,
                                                                                                      string recipientEmailAddress,
                                                                                                      CancellationToken cancellationToken)
        {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

            String requestUri = this.BuildRequestUrl($"/api/transactions/resendreceipt?applicationVersion={this.ApplicationInfoService.VersionString}");

            ResendReceiptRequestMessage requestBody = new()
            {
                Reference = reference,
                RecipientEmailAddress = recipientEmailAddress
            };

            Result<RecentActivityReceiptResendResponseDto>? responseDataResult = await this.Post<ResendReceiptRequestMessage, RecentActivityReceiptResendResponseDto>(requestUri, requestBody, accessToken.AccessToken, cancellationToken);

            if (responseDataResult.IsFailed)
            {
                Logger.LogInformation($"ResendRecentActivityReceipt failed {responseDataResult.Status}");
                return ResultHelpers.CreateFailure(responseDataResult);
            }

            RecentActivityReceiptResendResultModel model = new()
            {
                Success = responseDataResult.Data.Success,
                Message = responseDataResult.Data.Message,
                Reference = responseDataResult.Data.Reference,
                ReceiptReference = responseDataResult.Data.ReceiptReference,
                TransactionReference = responseDataResult.Data.TransactionReference
            };

            return model.Success
                ? Result.Success(model)
                : Result.Failure(model.Message ?? "Unable to resend the receipt.");
        }
    }

    public class RecentActivityReceiptSearchRequest
    {
        public string ApplicationVersion { get; set; }

        public int MerchantReportingId { get; set; }

        public DateTime ReportDate { get; set; }

        public string? SearchText { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }

    public class RecentActivityReceiptSearchResponseDto
    {
        public DateTime ReportDate { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<RecentActivityReceiptSearchItemDto> Items { get; set; } = [];
    }

    public class RecentActivityReceiptSearchItemDto
    {
        public string Reference { get; set; }

        public string TransactionType { get; set; }

        public string Product { get; set; }

        public string Operator { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public string ReceiptReference { get; set; }
    }

    public class RecentActivityReceiptResendResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Reference { get; set; }

        public string ReceiptReference { get; set; }

        public string TransactionReference { get; set; }
    }

    public class ResendReceiptRequestMessage
    {
        public string Reference { get; set; }

        public string RecipientEmailAddress { get; set; }
    }

    public class MerchantDailyPerformanceSummaryRequest
    {
        public int MerchantReportingId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class MerchantDailyPerformanceSummaryResponse
    {
        public List<MetricItem> Metrics { get; set; } = [];

        public List<DrillDownTransaction> DrillDownTransactions { get; set; } = [];
    }

    public class MetricItem
    {
        public string Title { get; set; }

        public decimal Value { get; set; }

        public string Description { get; set; }

        public int Category { get; set; }
    }

    public class DrillDownTransaction
    {
        public string Reference { get; set; }

        public string Product { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDateTime { get; set; }
    }

    public class MerchantTransactionMixSummaryResponseDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public TransactionMixBreakdown Breakdown { get; set; }

        public TransactionMixMeasure Measure { get; set; }

        public decimal TotalCount { get; set; }

        public decimal TotalValue { get; set; }

        public List<TransactionMixSummaryItemDto> Items { get; set; } = [];

        public List<TransactionMixDrillDownTransactionDto> DrillDownTransactions { get; set; } = [];
    }

    public class TransactionMixSummaryItemDto
    {
        public string Key { get; set; }

        public string Label { get; set; }

        public decimal Count { get; set; }

        public decimal Value { get; set; }
    }

    public class TransactionMixDrillDownTransactionDto
    {
        public string Reference { get; set; }

        public string TransactionType { get; set; }

        public string Product { get; set; }

        public string Operator { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDateTime { get; set; }
    }
}
