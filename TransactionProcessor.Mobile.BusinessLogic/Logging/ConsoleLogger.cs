using System.Diagnostics.CodeAnalysis;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Logging;

[ExcludeFromCodeCoverage]
public class ConsoleLogger : ILogger{
    private readonly ICorrelationIdProvider CorrelationIdProvider;

    #region Constructors

    public ConsoleLogger(ICorrelationIdProvider correlationIdProvider) {
        this.CorrelationIdProvider = correlationIdProvider;
        this.IsInitialised = true;
    }

    #endregion

    #region Properties

    public Boolean IsInitialised{ get; set; }

    #endregion

    #region Methods

    internal void Log(LogMessage logMessageModel)
    {
        List<LogMessage> logMessageModels = [logMessageModel];
        this.Log(logMessageModels);
    }

    internal void Log(List<LogMessage> logMessageModels)
    {
        String correlationId = this.CorrelationIdProvider.CorrelationId;
        foreach (LogMessage item in logMessageModels)
        {
            Console.WriteLine($"AppLog|{item.EntryDateTime}|Correlation Id: {correlationId}|{item.LogLevel}|{item.Message}");
        }
    }

    public void LogCritical(String message, Exception exception) => Log(LogMessage.CreateFatalLogMessages(message, exception));
    
    public void LogDebug(String message) => Log(LogMessage.CreateDebugLogMessage(message));
    
    public void LogError(String message, Exception exception) => Log(LogMessage.CreateErrorLogMessages(message, exception));

    public void LogInformation(String message) => this.Log(LogMessage.CreateInformationLogMessage(message));
    
    public void LogTrace(String message) => this.Log(LogMessage.CreateTraceLogMessage(message));

    public void LogWarning(String message) => this.Log(LogMessage.CreateWarningLogMessage(message));

    #endregion
}