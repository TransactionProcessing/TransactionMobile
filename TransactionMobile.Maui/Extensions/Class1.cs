using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.RequestHandlers;
using TransactionMobile.Maui.Requests;
using TransactionMobile.Maui.Services;
using TransactionMobile.Maui.ViewModels;

namespace TransactionMobile.Maui.Extensions
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginViewModel>();
            return builder;
        }

        public static MauiAppBuilder ConfigureRequestHandlers(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IRequestHandler<LoginRequest, String>, LoginRequestHandler>();
            builder.Services.AddSingleton<ServiceFactory>(ctx => { return t => ctx.GetService(t); });

            return builder;
        }        

        public static MauiAppBuilder ConfigureAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IAuthenticationService, DummyAuthenticationService>();

            return builder;
        }
    }


}
