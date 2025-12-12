using System.Net.Security;
using banditoth.MAUI.DeviceId;
using MediatR;
using SecurityService.Client;
using SimpleResults;
using TransactionMobile.Maui.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.Services.TrainingModeServices;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Admin;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using TransactionProcessor.Mobile.Pages;
using TransactionProcessor.Mobile.Pages.AppHome;
using TransactionProcessor.Mobile.Pages.MyAccount;
using TransactionProcessor.Mobile.Pages.Reports;
using TransactionProcessor.Mobile.Pages.Support;
using TransactionProcessor.Mobile.Pages.Transactions;
using TransactionProcessor.Mobile.Pages.Transactions.Admin;
using TransactionProcessor.Mobile.Pages.Transactions.BillPayment;
using TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;
using TransactionProcessor.Mobile.Pages.Transactions.Voucher;
//using TransactionProcessor.Mobile.Platforms.Android;
using TransactionProcessor.Mobile.UIServices;
using LogMessage = TransactionProcessor.Mobile.BusinessLogic.Models.LogMessage;
using ClientProxyBase;


#if ANDROID
using Javax.Net.Ssl;
using Xamarin.Android.Net;
using TransactionProcessor.Mobile.Platforms.Android;
#endif


namespace TransactionProcessor.Mobile.Extensions {
    public static class MauiAppBuilderExtensions {
        #region Methods

        public static MauiAppBuilder ConfigureDatabase(this MauiAppBuilder builder) {
            String connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "transactionpos1.db");
            Func< BusinessLogic.Database.LogLevel > logLevelFunc = new Func<BusinessLogic.Database.LogLevel>(() => BusinessLogic.Database.LogLevel.Warn);

            IDatabaseContext database = new DatabaseContext(connectionString, logLevelFunc);
            database.InitialiseDatabase();
            builder.Services.AddSingleton<IDatabaseContext>(database);

            return builder;
        }

        public static MauiAppBuilder ConfigureAppServices(this MauiAppBuilder builder) {
            //builder.Services.AddHttpClient()
            //    .AddHttpMessageHandler<CorrelationIdHandler>()
            //    .ConfigurePrimaryHttpMessageHandler(GetHandler);

            builder.Services.AddHttpClient("default")
                .AddHttpMessageHandler<CorrelationIdHandler>()
                .ConfigurePrimaryHttpMessageHandler(GetHandler);

            // Register as the default HttpClient
            builder.Services.AddTransient(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                return factory.CreateClient("default");
            });

            builder.Services.AddSingleton<Func<String, String>>(
                                                                new Func<String, String>(configSetting =>
                                                                                         {
                                                                                             IApplicationCache applicationCache = MauiProgram.Container.Services
                                                                                                 .GetService<IApplicationCache>();

                                                                                             if (configSetting == "ConfigServiceUrl")
                                                                                             {
                                                                                                 String configHostUrl = applicationCache.GetConfigHostUrl();
                                                                                                 if (String.IsNullOrEmpty(configHostUrl) == false)
                                                                                                 {
                                                                                                     return configHostUrl;
                                                                                                 }
                                                                                                 //return "https://sferguson.ddns.net:9200";
                                                                                                 return "http://192.168.1.167:9200";
                                                                                             }

                                                                                             Configuration configuration = applicationCache.GetConfiguration();

                                                                                             if (configuration != null)
                                                                                             {
                                                                                                 if (configSetting == "SecurityService")
                                                                                                 {
                                                                                                     return configuration.SecurityServiceUri;
                                                                                                 }

                                                                                                 if (configSetting == "TransactionProcessorACL")
                                                                                                 {
                                                                                                     return configuration.TransactionProcessorAclUri;
                                                                                                 }

                                                                                                 if (configSetting == "TransactionProcessorApi")
                                                                                                 {
                                                                                                     return configuration.TransactionProcessorUri;
                                                                                                 }

                                                                                                 if (configSetting == "EstateReportingApi")
                                                                                                 {
                                                                                                     return configuration.EstateReportingUri;
                                                                                                 }

                                                                                                 return string.Empty;
                                                                                             }

                                                                                             return string.Empty;
                                                                                         }));

            builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<IMerchantService, MerchantService>();

            builder.Services.AddSingleton<Func<Boolean, IConfigurationService>>(new Func<Boolean, IConfigurationService>(useTrainingMode =>
                                                                                {
                                                                                    if (useTrainingMode)
                                                                                    {
                                                                                        return new TrainingConfigurationService();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        return MauiProgram.Container.Services.GetService<IConfigurationService>();
                                                                                    }
                                                                                }));

            builder.Services.AddSingleton<Func<Boolean, IAuthenticationService>>(new Func<Boolean, IAuthenticationService>(useTrainingMode =>
                                                                                 {
                                                                                     if (useTrainingMode)
                                                                                     {
                                                                                         return new TrainingAuthenticationService();
                                                                                     }
                                                                                     else
                                                                                     {
                                                                                         return MauiProgram.Container.Services.GetService<IAuthenticationService>();
                                                                                     }
                                                                                 }));

            builder.Services.AddSingleton<Func<Boolean, ITransactionService>>(new Func<Boolean, ITransactionService>(useTrainingMode =>
                                                                              {
                                                                                  if (useTrainingMode)
                                                                                  {
                                                                                      return new TrainingTransactionService();
                                                                                  }
                                                                                  else
                                                                                  {
                                                                                      return MauiProgram.Container.Services.GetService<ITransactionService>();
                                                                                  }
                                                                              }));

            builder.Services.AddSingleton<Func<Boolean, IMerchantService>>(new Func<Boolean, IMerchantService>(useTrainingMode =>
                                                                                                               {
                                                                                                                   if (useTrainingMode)
                                                                                                                   {
                                                                                                                       return new TrainingMerchantService();
                                                                                                                   }
                                                                                                                   else
                                                                                                                   {
                                                                                                                       return MauiProgram.Container.Services.GetService<IMerchantService>();
                                                                                                                   }
                                                                                                               }));

            builder.Services.RegisterHttpClientX<ISecurityServiceClient, SecurityServiceClient>();
            builder.Services.AddSingleton<IApplicationCache, ApplicationCache>();

            builder.ConfigureDeviceIdProvider();

            return builder;
        }

        public static HttpMessageHandler GetHandler()
        {
#if ANDROID
            CustomAndroidMessageHandler androidMessageHandler = new()
            {

                ServerCertificateCustomValidationCallback = (sender,
                    certificate,
                    chain,
                    errors) => true,

            };
            return androidMessageHandler;
#else
                        HttpMessageHandler httpMessageHandler = new SocketsHttpHandler
                                                                {
                                                                    SslOptions = new SslClientAuthenticationOptions
                                                                                 {
                                                                                     RemoteCertificateValidationCallback = (sender,
                                                                                                                            certificate,
                                                                                                                            chain,
                                                                                                                            errors) => true,
                                                                                 }
                                                                };
                        return httpMessageHandler;
#endif
        }
        
        public static MauiAppBuilder ConfigureUIServices(this MauiAppBuilder builder) {
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
            builder.Services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
            builder.Services.AddSingleton<INavigationParameterService, NavigationParameterService>();
            return builder;
        }

        public static MauiAppBuilder ConfigureRequestHandlers(this MauiAppBuilder builder) {
            builder.Services.AddSingleton<MediatRServiceConfiguration>();
            builder.Services.AddSingleton<ISender, Mediator>();
            builder.Services.AddSingleton<IPublisher, Mediator>();
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IRequestHandler<GetConfigurationRequest, Result<Configuration>>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LoginRequest, Result<TokenResponseModel>>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<RefreshTokenRequest, Result<TokenResponseModel>>, LoginRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<GetContractProductsRequest, Result<List<ContractProductModel>>>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetMerchantBalanceRequest, Result<Decimal>>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetMerchantDetailsRequest, Result<MerchantDetailsModel>>, MerchantRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<PerformMobileTopupRequest, Result<PerformMobileTopupResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LogonTransactionRequest, Result<PerformLogonResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformVoucherIssueRequest, Result<PerformVoucherIssueResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformReconciliationRequest, Result<PerformReconciliationResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformBillPaymentGetAccountRequest, Result<PerformBillPaymentGetAccountResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformBillPaymentGetMeterRequest, Result<PerformBillPaymentGetMeterResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformBillPaymentMakePostPaymentRequest, Result<PerformBillPaymentMakePaymentResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformBillPaymentMakePrePaymentRequest, Result<PerformBillPaymentMakePaymentResponseModel>>, TransactionRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<UploadLogsRequest, Boolean>, SupportRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<ViewLogsRequest, List<LogMessage>>, SupportRequestHandler>();

            return builder;
        }

        public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder) {
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<TransactionsPageViewModel>();

            builder.Services.AddTransient<MobileTopupSelectOperatorPageViewModel>();
            builder.Services.AddTransient<MobileTopupSelectProductPageViewModel>();
            builder.Services.AddTransient<MobileTopupPerformTopupPageViewModel>();
            builder.Services.AddTransient<MobileTopupSuccessPageViewModel>();
            builder.Services.AddTransient<MobileTopupFailedPageViewModel>();

            builder.Services.AddTransient<VoucherSelectOperatorPageViewModel>();
            builder.Services.AddTransient<VoucherSelectProductPageViewModel>();
            builder.Services.AddTransient<VoucherPerformIssuePageViewModel>();
            builder.Services.AddTransient<VoucherIssueSuccessPageViewModel>();
            builder.Services.AddTransient<VoucherIssueFailedPageViewModel>();

            builder.Services.AddTransient<BillPaymentSelectOperatorPageViewModel>();
            builder.Services.AddTransient<BillPaymentSelectProductPageViewModel>();
            builder.Services.AddTransient<BillPaymentGetAccountPageViewModel>();
            builder.Services.AddTransient<BillPaymentGetMeterPageViewModel>();
            builder.Services.AddTransient<BillPaymentPayBillPageViewModel>();
            builder.Services.AddTransient<BillPaymentSuccessPageViewModel>();
            builder.Services.AddTransient<BillPaymentFailedPageViewModel>();

            builder.Services.AddTransient<AdminPageViewModel>();

            builder.Services.AddTransient<SupportPageViewModel>();
            builder.Services.AddTransient<ViewLogsPageViewModel>();

            builder.Services.AddTransient<MyAccountPageViewModel>();
            builder.Services.AddTransient<MyAccountAddressPageViewModel>();
            builder.Services.AddTransient<MyAccountContactPageViewModel>();
            builder.Services.AddTransient<MyAccountDetailsPageViewModel>();
            
            builder.Services.AddTransient<ReportsPageViewModel>();
            builder.Services.AddTransient<ReportsBalanceAnalysisPageViewModel>();
            builder.Services.AddTransient<ReportsSalesAnalysisPageViewModel>();

            return builder;
        }

        public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder) {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<TransactionsPage>();

            builder.Services.AddTransient<MobileTopupSelectOperatorPage>();
            builder.Services.AddTransient<MobileTopupSelectProductPage>();
            builder.Services.AddTransient<MobileTopupPerformTopupPage>();
            builder.Services.AddTransient<MobileTopupSuccessPage>();
            builder.Services.AddTransient<MobileTopupFailedPage>();

            builder.Services.AddTransient<VoucherSelectOperatorPage>();
            builder.Services.AddTransient<VoucherSelectProductPage>();
            builder.Services.AddTransient<VoucherPerformIssuePage>();
            builder.Services.AddTransient<VoucherIssueSuccessPage>();
            builder.Services.AddTransient<VoucherIssueFailedPage>();

            builder.Services.AddTransient<BillPaymentSelectOperatorPage>();
            builder.Services.AddTransient<BillPaymentSelectProductPage>();
            builder.Services.AddTransient<BillPaymentGetAccountPage>();
            builder.Services.AddTransient<BillPaymentGetMeterPage>();
            builder.Services.AddTransient<BillPaymentPayBillPage>();
            builder.Services.AddTransient<BillPaymentSuccessPage>();
            builder.Services.AddTransient<BillPaymentFailedPage>();

            builder.Services.AddTransient<AdminPage>();

            builder.Services.AddTransient<SupportPage>();
            builder.Services.AddTransient<ViewLogsPage>();

            builder.Services.AddTransient<MyAccountPage>();
            builder.Services.AddTransient<MyAccountAddressesPage>();
            builder.Services.AddTransient<MyAccountContactPage>();
            builder.Services.AddTransient<MyAccountDetailsPage>();

            builder.Services.AddTransient<ReportsPage>();
            builder.Services.AddTransient<ReportsBalanceAnalysisPage>();
            builder.Services.AddTransient<ReportsSalesAnalysisPage>();
            
            return builder;
        }

        #endregion

        public static IHttpClientBuilder RegisterHttpClientX<TInterface, TImplementation>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddTransient<CorrelationIdHandler>();

            services.AddHttpClient<TInterface, TImplementation>()
                .AddHttpMessageHandler<CorrelationIdHandler>()
                .ConfigurePrimaryHttpMessageHandler(GetHandler);

            return services.AddHttpClient<TImplementation>();
        }
    }

#if ANDROID
    public class CustomAndroidMessageHandler : AndroidMessageHandler {
        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection) {
            return new DangerousHostNameVerifier();
        }
    }
#endif
}