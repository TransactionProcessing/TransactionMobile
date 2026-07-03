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
}
