namespace TransactionMobile.Maui.Extensions
{
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using MediatR;
    using ViewModels;

    public static class MauiAppBuilderExtensions
    {
        #region Methods

        public static MauiAppBuilder ConfigureAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IAuthenticationService, DummyAuthenticationService>();

            return builder;
        }

        public static MauiAppBuilder ConfigureRequestHandlers(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IRequestHandler<LoginRequest, String>, LoginRequestHandler>();
            builder.Services.AddSingleton<ServiceFactory>(ctx => { return t => ctx.GetService(t); });

            return builder;
        }

        public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginViewModel>();
            return builder;
        }

        #endregion
    }
}