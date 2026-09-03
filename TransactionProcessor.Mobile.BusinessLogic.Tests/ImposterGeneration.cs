global using Configuration = TransactionProcessor.Mobile.BusinessLogic.Models.Configuration;
global using TokenResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.TokenResponseModel;
global using ContractProductModel = TransactionProcessor.Mobile.BusinessLogic.Models.ContractProductModel;
global using ContractOperatorModel = TransactionProcessor.Mobile.BusinessLogic.Models.ContractOperatorModel;
global using PerformLogonResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformLogonResponseModel;
global using PerformMobileTopupResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformMobileTopupResponseModel;
global using PerformVoucherIssueResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformVoucherIssueResponseModel;
global using PerformReconciliationResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformReconciliationResponseModel;
global using PerformBillPaymentGetAccountResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformBillPaymentGetAccountResponseModel;
global using PerformBillPaymentGetMeterResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformBillPaymentGetMeterResponseModel;
global using PerformBillPaymentMakePaymentResponseModel = TransactionProcessor.Mobile.BusinessLogic.Models.PerformBillPaymentMakePaymentResponseModel;
global using DailyPerformanceSummaryModel = TransactionProcessor.Mobile.BusinessLogic.Models.DailyPerformanceSummaryModel;
global using TransactionMixSummaryModel = TransactionProcessor.Mobile.BusinessLogic.Models.TransactionMixSummaryModel;
global using RecentActivityReceiptReportModel = TransactionProcessor.Mobile.BusinessLogic.Models.RecentActivityReceiptReportModel;

using Imposter.Abstractions;

[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IApplicationCache))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.IApplicationInfoService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.IApplicationThemeService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.IApplicationUpdateLauncherService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IAuthenticationService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IBalanceRefresher))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IConfigurationService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Database.IDatabaseContext))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.IDeviceService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.IDialogService))]
[assembly: GenerateImposter(typeof(MediatR.IMediator))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IMerchantService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.INavigationParameterService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.INavigationService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IReportsService))]
[assembly: GenerateImposter(typeof(SecurityService.Client.ISecurityServiceClient))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.UIServices.ISentryService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.ITransactionService))]
[assembly: GenerateImposter(typeof(TransactionProcessor.Mobile.BusinessLogic.Services.IUpdateService))]
