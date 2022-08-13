namespace TransactionMobile.Maui.Extensions
{
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Extensions.Caching.Memory;
    using BusinessLogic.Models;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using BusinessLogic.Services.DummyServices;
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
                return Database.LogLevel.Debug;
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
                                                                                             if (configSetting == "ConfigServiceUrl")
                                                                                             {
                                                                                                 return "https://5r8nmm.deta.dev";
                                                                                             }

                                                                                             IMemoryCacheService memoryCacheService = MauiProgram.Container.Services
                                                                                                 .GetService<IMemoryCacheService>();

                                                                                             Boolean configFound = memoryCacheService.TryGetValue<Configuration>("Configuration", out Configuration configuration);

                                                                                             if (configFound && configuration != null)
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
                    return new DummyConfigurationService();
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
                    return new DummyAuthenticationService();
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
                    return new DummyTransactionService();
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
                    return new DummyMerchantService();
                }
                else
                {
                    return MauiProgram.Container.Services.GetService<IMerchantService>();
                }
            }));

            builder.Services.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
            builder.Services.AddSingleton<IEstateClient, EstateClient>();
            builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

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
            builder.Services.AddSingleton<IRequestHandler<GetConfigurationRequest, Configuration>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LoginRequest, TokenResponseModel>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<RefreshTokenRequest, TokenResponseModel>, LoginRequestHandler>();

            builder.Services.AddSingleton<IRequestHandler<GetContractProductsRequest, List<ContractProductModel>>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetMerchantBalanceRequest, Decimal>, MerchantRequestHandler>();
            
            builder.Services.AddSingleton<IRequestHandler<PerformMobileTopupRequest, Boolean>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LogonTransactionRequest, PerformLogonResponseModel>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformVoucherIssueRequest, Boolean>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformReconciliationRequest, Boolean>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<UploadLogsRequest, Boolean>, SupportRequestHandler>();

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

            builder.Services.AddTransient<AdminPageViewModel>();

            builder.Services.AddTransient<SupportPageViewModel>();

            builder.Services.AddTransient<MyAccountPageViewModel>();

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

            builder.Services.AddTransient<AdminPage>();

            builder.Services.AddTransient<SupportPage>();

            builder.Services.AddTransient<MyAccountPage>();

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