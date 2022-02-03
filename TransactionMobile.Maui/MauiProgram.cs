using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

public static class MauiProgram
{
	public static MauiApp Container;
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureRequestHandlers()
			.ConfigureViewModels()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		Container = builder.Build();
		return Container;
	}
}
