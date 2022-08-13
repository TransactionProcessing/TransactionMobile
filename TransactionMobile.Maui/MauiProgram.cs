using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Caching.Memory;
using UIServices;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;
using Shared.Logger;

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
		
		Container = Builder.Build();

		// Setup static logger
		IDatabaseContext databaseContext = MauiProgram.Container.Services.GetService<IDatabaseContext>();
		Logger.Initialise(new DatabaseLogger(databaseContext));

		return Container;
	}
}
