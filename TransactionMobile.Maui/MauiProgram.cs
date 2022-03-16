using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Caching.Memory;
using UIServices;

public static class MauiProgram
{
	public static MauiApp Container;
	public static MauiApp CreateMauiApp()
	{
#if ANDROID && DEBUG
        Platforms.Services.DangerousAndroidMessageHandlerEmitter.Register();
        Platforms.Services.DangerousTrustProvider.Register();
#endif

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
