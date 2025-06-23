using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class AddressModel
{
    public String AddressLine1 { get; set; }
    public String AddressLine2 { get; set; }
    public String AddressLine3 { get; set; }
    public String AddressLine4 { get; set; }
    public String PostalCode { get; set; }
    public String Region { get; set; }
    public String Town { get; set; }
}

[ExcludeFromCodeCoverage]
public class LogMessage
{
    #region Properties

    public DateTime EntryDateTime { get; set; }
    public Int32 Id { get; set; }
    public LogLevel LogLevel { get; set; }
    public String LogLevelString { get; set; }
    public String Message { get; set; }

    #endregion

    private static LogMessage CreateLogMessage(String message,
                                               LogLevel logLevel)
    {
        return new LogMessage
        {
            EntryDateTime = DateTime.UtcNow,
            Message = message,
            LogLevel = logLevel,
            LogLevelString = logLevel.ToString()
        };
    }

    /// <summary>
    /// Creates the debug log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static LogMessage CreateDebugLogMessage(String message)
    {
        return LogMessage.CreateLogMessage(message, Models.LogLevel.Debug);
    }

    /// <summary>
    /// Creates the error log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static LogMessage CreateErrorLogMessage(String message)
    {
        return LogMessage.CreateLogMessage(message, Models.LogLevel.Error);
    }

    /// <summary>
    /// Creates the error log messages.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns></returns>
    public static List<LogMessage> CreateErrorLogMessages(String message, Exception exception)
    {
        List<LogMessage> logMessages = new List<LogMessage>();
        logMessages.Add(LogMessage.CreateLogMessage(message, Models.LogLevel.Error));
        Exception e = exception;
        while (e != null)
        {
            logMessages.Add(LogMessage.CreateLogMessage(e.Message, Models.LogLevel.Error));
            e = e.InnerException;
        }

        return logMessages;
    }

    /// <summary>
    /// Creates the fatal log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static LogMessage CreateFatalLogMessage(String message)
    {
        return LogMessage.CreateLogMessage(message, Models.LogLevel.Fatal);
    }

    /// <summary>
    /// Creates the fatal log messages.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns></returns>
    public static List<LogMessage> CreateFatalLogMessages(String message, Exception exception)
    {
        List<LogMessage> logMessages = new List<LogMessage>();
        logMessages.Add(LogMessage.CreateLogMessage(message, Models.LogLevel.Fatal));
        Exception e = exception;
        while (e != null)
        {
            logMessages.Add(LogMessage.CreateLogMessage(e.Message, Models.LogLevel.Fatal));
            e = e.InnerException;
        }

        return logMessages;
    }

    /// <summary>
    /// Creates the information log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static LogMessage CreateInformationLogMessage(String message)
    {
        return LogMessage.CreateLogMessage(message, Models.LogLevel.Info);
    }

    /// <summary>
    /// Creates the trace log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static LogMessage CreateTraceLogMessage(String message)
    {
        return LogMessage.CreateLogMessage(message, Models.LogLevel.Trace);
    }

    /// <summary>
    /// Creates the warning log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static LogMessage CreateWarningLogMessage(String message)
    {
        return LogMessage.CreateLogMessage(message, Models.LogLevel.Warn);
    }
}

public enum LogLevel
{
    Fatal,

    Error,

    Warn,

    Info,

    Debug,

    Trace
}