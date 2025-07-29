using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.Extensions;
using TransactionProcessor.Mobile.UIServices;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionProcessor.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp Container;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureRequestHandlers()
                .ConfigurePages()
                .ConfigureViewModels()
                .ConfigureAppServices()
                .ConfigureUIServices()
                .ConfigureDatabase()
                .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            }).Services.AddTransient<IDeviceService, DeviceService>()
                .AddMemoryCache();

            builder.Logging.SetMinimumLevel(LogLevel.Trace);//.AddConsole();
            
            Container = builder.Build();

            Logger.Initialise(new ConsoleLogger());

            return Container;
        }
    }

    public class CorrelationIdHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            String correlationId = CorrelationIdProvider.CorrelationId ?? Guid.NewGuid().ToString();
            CorrelationIdProvider.CorrelationId = correlationId;

            request.Headers.Add("correlationId", correlationId);
            return base.SendAsync(request, cancellationToken);
        }
    }

    }
