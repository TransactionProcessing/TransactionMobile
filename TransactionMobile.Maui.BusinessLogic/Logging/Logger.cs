using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Logging;

[ExcludeFromCodeCoverage]
public static class Logger
{
    #region Fields

    /// <summary>
    /// The logger object
    /// </summary>
    private static List<ILogger> LoggerObjects;

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

    /// <summary>
    /// Initialises the specified logger object.
    /// </summary>
    /// <param name="loggerObject">The logger object.</param>
    /// <exception cref="ArgumentNullException">loggerObject</exception>
    public static void Initialise(ILogger loggerObject)
    {
        if (loggerObject == null) {
            throw new ArgumentNullException(nameof(loggerObject));
        }

        if (Logger.LoggerObjects == null) {
            Logger.LoggerObjects = new List<ILogger>();
        }
        Logger.LoggerObjects.Add(loggerObject);

        Logger.IsInitialised = true;
    }

    /// <summary>
    /// Logs the critical.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public static void LogCritical(String message, Exception exception)
    {
        foreach (ILogger loggerObject in Logger.LoggerObjects) {
            loggerObject.LogCritical(message, exception);
        }
            
    }

    /// <summary>
    /// Logs the debug.
    /// </summary>
    /// <param name="message">The message.</param>
    public static void LogDebug(String message)
    {
        foreach (ILogger loggerObject in Logger.LoggerObjects)
        {
            loggerObject.LogDebug(message);
        }
    }

    /// <summary>
    /// Logs the error.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public static void LogError(String message, Exception exception)
    {
        foreach (ILogger loggerObject in Logger.LoggerObjects) {
            loggerObject.LogError(message, exception);
        }
    }

    /// <summary>
    /// Logs the information.
    /// </summary>
    /// <param name="message">The message.</param>
    public static void LogInformation(String message)
    {
        if (Logger.LoggerObjects == null || Logger.LoggerObjects.Any() == false){
            Console.WriteLine(message);
            return;
        }
        foreach (ILogger loggerObject in Logger.LoggerObjects)
        {
            loggerObject.LogInformation(message);
        }
    }

    /// <summary>
    /// Logs the trace.
    /// </summary>
    /// <param name="message">The message.</param>
    public static void LogTrace(String message)
    {
        foreach (ILogger loggerObject in Logger.LoggerObjects) {
            loggerObject.LogTrace(message);
        }
    }

    /// <summary>
    /// Logs the warning.
    /// </summary>
    /// <param name="message">The message.</param>
    public static void LogWarning(String message)
    {
        foreach (ILogger loggerObject in Logger.LoggerObjects)
        {
            loggerObject.LogWarning(message);
        }
    }

    #endregion
}