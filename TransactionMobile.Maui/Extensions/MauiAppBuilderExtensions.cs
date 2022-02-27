namespace TransactionMobile.Maui.Extensions
{
    using BusinessLogic.Models;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using BusinessLogic.ViewModels;
    using BusinessLogic.ViewModels.Support;
    using BusinessLogic.ViewModels.Transactions;
    using MediatR;
    using UIServices;

    public static class MauiAppBuilderExtensions
    {
        #region Methods

        public static MauiAppBuilder ConfigureAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IAuthenticationService, DummyAuthenticationService>();
            builder.Services.AddSingleton<IMerchantService, DummyMerchantService>();
            builder.Services.AddSingleton<ITransactionService, DummyTransactionService>();

            return builder;
        }

        public static MauiAppBuilder ConfigureUIServices(this MauiAppBuilder builder)
        {
        //    builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
            return builder;
        }

        public static MauiAppBuilder ConfigureRequestHandlers(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IRequestHandler<LoginRequest, String>, LoginRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetContractProductsRequest, List<ContractProductModel>>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<GetMerchantBalanceRequest, Decimal>, MerchantRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<PerformMobileTopupRequest, Boolean>, TransactionRequestHandler>();
            builder.Services.AddSingleton<IRequestHandler<LogonTransactionRequest, Boolean>, TransactionRequestHandler>();
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