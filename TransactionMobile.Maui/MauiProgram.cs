using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
//using SQLitePCL;
using UIServices;

public static class MauiProgram
{
	public static MauiApp Container;
	public static MauiApp CreateMauiApp()
	{
        //raw.SetProvider(new SQLite3Provider_sqlite3());
		var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureRequestHandlers().ConfigureViewModels().ConfigureAppServices().ConfigureUIServices().UseMauiCommunityToolkit().ConfigureDatabase()
			   .ConfigureFonts(fonts =>
                               {
                                   fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                               })
			.Services.AddTransient<IDeviceService, DeviceService>()
               .AddMemoryCache();

		Container = builder.Build();

		return Container;
	}
}
