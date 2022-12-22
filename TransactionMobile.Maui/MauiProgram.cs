using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;
using UIServices;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;

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

        Builder.Logging.AddConsoleLogger(_ => { });

		Container = Builder.Build();

		// Setup static logger
		IDatabaseContext databaseContext = MauiProgram.Container.Services.GetService<IDatabaseContext>();
        IApplicationCache applicationCache = MauiProgram.Container.Services.GetService<IApplicationCache>();
        
		return Container;
	}
}
