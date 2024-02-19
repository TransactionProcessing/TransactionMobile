namespace TransactionMobile.Maui.BusinessLogic.Logging;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class NullLogger : ILogger{
    #region Properties

    /// <summary>
    /// Instances this instance.
    /// </summary>
    /// <returns></returns>
    public static NullLogger Instance{
        get{
            return new NullLogger();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is initialised.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is initialised; otherwise, <c>false</c>.
    /// </value>
    public Boolean IsInitialised{ get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Initialises this instance.
    /// </summary>
    public void Initialise(){
        this.IsInitialised = true;
    }

    /// <summary>
    /// Logs the critical.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public void LogCritical(String message, Exception exception){
    }

    /// <summary>
    /// Logs the debug.
    /// </summary>
    /// <param name="message">The message.</param>
    public void LogDebug(String message){
    }

    /// <summary>
    /// Logs the error.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public void LogError(String message, Exception exception){
    }

    /// <summary>
    /// Logs the information.
    /// </summary>
    /// <param name="message">The message.</param>
    public void LogInformation(String message){
    }

    /// <summary>
    /// Logs the trace.
    /// </summary>
    /// <param name="message">The message.</param>
    public void LogTrace(String message){
    }

    /// <summary>
    /// Logs the warning.
    /// </summary>
    /// <param name="message">The message.</param>
    public void LogWarning(String message){
    }

    #endregion
}