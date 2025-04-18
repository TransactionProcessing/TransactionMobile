﻿using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.Logging;
using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using MetroLog.MicrosoftExtensions;
//using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using UIServices;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

public static class MauiProgram
{
	public static MauiApp Container;
	private static MauiAppBuilder Builder;
	public static MauiApp CreateMauiApp()
	{
        Logger.LogInformation("In CreateMauiApp");

        Builder = MauiApp.CreateBuilder();
		Builder.UseMauiApp<App>()
               .UseSkiaSharp()
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
        MauiProgram.Builder.Logging.SetMinimumLevel(LogLevel.Trace);//.AddConsole();
        
		Logger.Initialise(new ConsoleLogger());

		Container = Builder.Build();

		return Container;
	}
}
