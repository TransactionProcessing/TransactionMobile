namespace TransactionProcessor.Mobile.BusinessLogic.Logging
{
    public interface ILogger
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialised; otherwise, <c>false</c>.
        /// </value>
        Boolean IsInitialised { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Logs the critical.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void LogCritical(String message, Exception exception);

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogDebug(String message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void LogError(String message, Exception exception);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogInformation(String message);

        /// <summary>
        /// Logs the trace.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogTrace(String message);

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogWarning(String message);

        #endregion
    }
}
