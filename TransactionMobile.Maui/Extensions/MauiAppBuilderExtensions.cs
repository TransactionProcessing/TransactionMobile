namespace TransactionMobile.Maui.Extensions
{

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
    using MediatR;
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
            builder.Services.AddSingleton<IAuthenticationService, DummyAuthenticationService>();
            builder.Services.AddSingleton<IMerchantService, DummyMerchantService>();
            builder.Services.AddSingleton<ITransactionService, DummyTransactionService>();
            builder.Services.AddSingleton<IConfigurationService, DummyConfigurationService>();

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
            builder.Services.AddSingleton<IRequestHandler<PerformReconciliationRequest, Boolean>, TransactionRequestHandler>();

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


            return builder;
        }

        #endregion
    }
}