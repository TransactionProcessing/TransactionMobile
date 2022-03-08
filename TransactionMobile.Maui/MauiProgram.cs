using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using UIServices;

public static class MauiProgram
{
	public static MauiApp Container;
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureRequestHandlers().ConfigureViewModels().ConfigureAppServices().ConfigureUIServices().UseMauiCommunityToolkit()
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
