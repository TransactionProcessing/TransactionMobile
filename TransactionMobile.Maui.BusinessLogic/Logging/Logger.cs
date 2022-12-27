namespace Shared.Logger
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 
    /// </summary>
    public static class Logger
    {
        #region Fields

        /// <summary>
        /// The logger object
        /// </summary>
        private static ILogger LoggerObject;

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
            Logger.LoggerObject = loggerObject ?? throw new ArgumentNullException(nameof(loggerObject));

            Logger.IsInitialised = true;
        }

        /// <summary>
        /// Logs the critical.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogCritical(Exception exception)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.LogCritical(exception);
        }

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogDebug(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.LogDebug(message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogError(Exception exception)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.LogError(exception);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogInformation(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.LogInformation(message);
        }

        /// <summary>
        /// Logs the trace.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogTrace(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.LogTrace(message);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogWarning(String message)
        {
            Logger.ValidateLoggerObject();

            Logger.LoggerObject.LogWarning(message);
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