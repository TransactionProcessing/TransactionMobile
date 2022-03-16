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
    using BusinessLogic.ViewModels.Support;
    using BusinessLogic.ViewModels.Transactions;
    using Database;
    using EstateManagement.Client;
    using MediatR;
    using SecurityService.Client;
    using UIServices;

    public static class MauiAppBuilderExtensions
    {
        #region Methods

        public static MauiAppBuilder ConfigureDatabase(this MauiAppBuilder builder)
        {
            String connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "transactionpos.db");
            IDatabaseContext database = new DatabaseContext(connectionString);
            database.InitialiseDatabase(); 
            builder.Services.AddSingleton<IDatabaseContext>(database);

            return builder;
        }
        
        public static MauiAppBuilder ConfigureAppServices(this MauiAppBuilder builder)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message,
                                                             certificate2,
                                                             arg3,
                                                             arg4) =>
                                                            {
                                                                return true;
                                                            }
            };
            
            HttpClient httpClient = new HttpClient(httpClientHandler);
            builder.Services.AddSingleton<HttpClient>(httpClient);
            builder.Services.AddSingleton<Func<String, String>>(
                                                                new Func<String, String>(configSetting =>
                                                                                         {
                                                                                             

                                                                                             if (configSetting == "ConfigServiceUrl")
                                                                                             {
                                                                                                 return "https://5r8nmm.deta.dev";
                                                                                             }

                                                                                             IMemoryCache cacheprovider = MauiProgram.Container.Services
                                                                                                 .GetService<IMemoryCache>();

                                                                                             Configuration configuration = cacheprovider.Get<Configuration>("Configuration");

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

            //builder.Services.AddSingleton<IConfigurationService, DummyConfigurationService>();
            //builder.Services.AddSingleton<IAuthenticationService, DummyAuthenticationService>();
            //builder.Services.AddSingleton<ITransactionService, DummyTransactionService>();
            //builder.Services.AddSingleton<IMerchantService, DummyMerchantService>();

            builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<IMerchantService, MerchantService>();

            builder.Services.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
            builder.Services.AddSingleton<IEstateClient, EstateClient>();

            return builder;
        }

        public static MauiAppBuilder ConfigureUIServices(this MauiAppBuilder builder)
        {
        //    builder.Services.AddSingleton<IDialogService, DialogService>();
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

            builder.Services.AddTransient<SupportPageViewModel>();


            return builder;
        }

        #endregion
    }
}