using TransactionMobile.Maui.Extensions;

namespace TransactionMobile.Maui;

using BusinessLogic.UIServices;
using CommunityToolkit.Maui;
using MetroLog.MicrosoftExtensions;
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

        var logger = MauiProgram.Container.Services.GetService<MetroLog.ILogger>();
        Logger.Initialise(logger);

		return Container;
	}


    public static class Logger
    {
        #region Fields

        /// <summary>
        /// The logger object
        /// </summary>
        private static MetroLog.ILogger LoggerObject;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialised; otherwise, <c>false</c>.
        /// </value>
        public static Boolean IsInitialised { get; set; }

        #endregion

        #region Methods

        public static void Initialise(MetroLog.ILogger loggerObject)
        {
            Logger.LoggerObject = loggerObject ?? throw new ArgumentNullException(nameof(loggerObject));

            Logger.IsInitialised = true;
        }

        /// <summary>
        /// Logs the critical.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogCritical(Exception exception)
        {
            Logger.LoggerObject.Fatal("", exception);
        }

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogDebug(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.Debug(message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogError(Exception exception)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.Error("",exception);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogInformation(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.Info(message);
        }

        /// <summary>
        /// Logs the trace.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogTrace(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.Trace(message);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogWarning(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.Warn(message);
        }

        /// <summary>
        /// Validates the logger object.
        /// </summary>
        /// <exception cref="InvalidOperationException">Logger has not been initialised</exception>
        private static void ValidateLoggerObject()
        {
            if (Logger.LoggerObject == null)
            {
                throw new InvalidOperationException("Logger has not been initialised");
            }
        }

        #endregion
    }
}
