using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.Extensions;
using TransactionProcessor.Mobile.UIServices;

namespace TransactionProcessor.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp Container;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseLiveCharts()
                .UseSkiaSharp()
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

            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            
            Container = builder.Build();

            var serialiser = Container.Services.GetRequiredService<IStringSerialiser>();
            StringSerialiser.Initialise(serialiser);

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
