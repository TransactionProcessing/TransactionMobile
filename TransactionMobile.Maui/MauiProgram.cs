using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using UIServices;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;
using Shared.Logger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

public static class MauiProgram
{
	public static MauiApp Container;
	private static MauiAppBuilder Builder;
	public static MauiApp CreateMauiApp()
	{
		Builder = MauiApp.CreateBuilder();
		Builder.UseMauiApp<App>()
			.ConfigureRequestHandlers()
			.ConfigurePages()
			.ConfigureViewModels()
			.ConfigureAppServices()
			.ConfigureUIServices()
			.UseMauiCommunityToolkit()
			.ConfigureDatabase()
			   .ConfigureFonts(fonts =>
							   {
								   fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
							   })
			.Services.AddTransient<IDeviceService, DeviceService>()
			   .AddMemoryCache();

        Builder.Services.AddLogging(o => {
                                        o.AddConsole();
                                        o.SetMinimumLevel(LogLevel.Trace);
                                    });

		Container = Builder.Build();

		// Setup static logger
		//IDatabaseContext databaseContext = MauiProgram.Container.Services.GetService<IDatabaseContext>();
        //IApplicationCache applicationCache = MauiProgram.Container.Services.GetService<IApplicationCache>();
        //Logger.Initialise(new DatabaseLogger(databaseContext,applicationCache));
		var logger = MauiProgram.Container.Services.GetService<ILogger>();


		return Container;
	}
}
