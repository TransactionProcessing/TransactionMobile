namespace TransactionMobile.Maui.Extensions
{
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Extensions.Caching.Memory;
    using BusinessLogic.Models;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using BusinessLogic.UIServices;
    using BusinessLogic.ViewModels;
    using BusinessLogic.ViewModels.Admin;
    using BusinessLogic.ViewModels.Support;
    using BusinessLogic.ViewModels.Transactions;
    using Database;
    using EstateManagement.Client;
    using MediatR;
    using SecurityService.Client;
    using UIServices;
    using TransactionMobile.Maui.Pages;
    using TransactionMobile.Maui.Pages.Transactions;
    using TransactionMobile.Maui.Pages.Transactions.MobileTopup;
    using TransactionMobile.Maui.Pages.Transactions.Voucher;
    using TransactionMobile.Maui.Pages.Transactions.Admin;
    using TransactionMobile.Maui.Pages.Support;
    using System;
    using BusinessLogic.ViewModels.MyAccount;
    using Pages.AppHome;
    using Pages.MyAccount;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using BusinessLogic.Services.TrainingModeServices;
    using Pages.Transactions.BillPayment;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using LogMessage = BusinessLogic.Models.LogMessage;
#if ANDROID
    using Javax.Net.Ssl;
    using Platforms.Services;
    using Xamarin.Android.Net;
#endif
    public static class MauiAppBuilderExtensions
    {
        #region Methods

        public static MauiAppBuilder ConfigureDatabase(this MauiAppBuilder builder)
        {
            String connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "transactionpos1.db");
            Func<Database.LogLevel> logLevelFunc = new Func<Database.LogLevel>( () =>
                {
                return Database.LogLevel.Warn;
            });                      

            IDatabaseContext database = new DatabaseContext(connectionString, logLevelFunc);
            database.InitialiseDatabase(); 
            builder.Services.AddSingleton<IDatabaseContext>(database);
                        
            return builder;
        }

        public static MauiAppBuilder ConfigureAppServices(this MauiAppBuilder builder) {
            
            HttpClient httpClient = CreateHttpClient();
            builder.Services.AddSingleton<HttpClient>(httpClient);
            builder.Services.AddSingleton<Func<String, String>>(
                                                                new Func<String, String>(configSetting =>
                                                                                         {
                                                                                             IApplicationCache applicationCache = MauiProgram.Container.Services
                                                                                                 .GetService<IApplicationCache>();

                                                                                             if (configSetting == "ConfigServiceUrl") {
                                                                                                 String configHostUrl = applicationCache.GetConfigHostUrl();
                                                                                                 if (String.IsNullOrEmpty(configHostUrl) == false) {
                                                                                                     return configHostUrl;
                                                                                                 }
                                                                                                 return "https://sferguson.ddns.net:9200";
                                                                                                 //return "http://192.168.0.10:9200";
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

                                                                                                 if (configSetting == "EstateManagementApi")
                                                                                                 {
                                                                                                     return configuration.EstateManagementUri;
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

            builder.Services.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
            builder.Services.AddSingleton<IEstateClient, EstateClient>();
            builder.Services.AddSingleton<IApplicationCache, ApplicationCache>();

            return builder;
        }

        private static HttpClient CreateHttpClient() {

#if ANDROID
            CustomAndroidMessageHandler androidMessageHandler = new() {

                                                                          ServerCertificateCustomValidationCallback = (sender,
                                                                              certificate,
                                                                              chain,
                                                                              errors) => true,

                                                                      };
            return new HttpClient(androidMessageHandler);
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
            return new HttpClient(httpMessageHandler);
#endif
        }

        public static MauiAppBuilder ConfigureUIServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
            builder.Services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
            return builder;
        }

        public static MauiAppBuilder ConfigureRequestHandlers(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IRequestHandler<GetConfigurationRequest, Result<Configuration>>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LoginRequest, Result<TokenResponseModel>>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<RefreshTokenRequest, Result<TokenResponseModel>>, LoginRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<GetContractProductsRequest, Result<List<ContractProductModel>>>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetMerchantBalanceRequest, Result<Decimal>>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetMerchantDetailsRequest, Result<MerchantDetailsModel>>, MerchantRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<PerformMobileTopupRequest, Result<SaleTransactionResponseMessage>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LogonTransactionRequest, Result<PerformLogonResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformVoucherIssueRequest, Result<SaleTransactionResponseMessage>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformReconciliationRequest, Result<ReconciliationResponseMessage>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformBillPaymentGetAccountRequest, Result<PerformBillPaymentGetAccountResponseModel>>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformBillPaymentMakePaymentRequest, Result<SaleTransactionResponseMessage>>, TransactionRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<UploadLogsRequest, Boolean>, SupportRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<ViewLogsRequest, List<LogMessage>>, SupportRequestHandler>();

            builder.Services.AddSingleton<ServiceFactory>(ctx => { return t => ctx.GetService(t); });

            return builder;
        }

        public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPageViewModel>();
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

            builder.Services.AddTransient<HomePageViewModel>();

            return builder;
        }

        public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>();
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

            builder.Services.AddTransient<HomePage>();

            return builder;
        }

        #endregion
    }

#if ANDROID
    public class CustomAndroidMessageHandler : AndroidMessageHandler
    {
        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection) {
            return new DangerousHostNameVerifier();
        }
    }
#endif
}