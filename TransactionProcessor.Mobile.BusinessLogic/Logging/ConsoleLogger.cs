using System.Diagnostics.CodeAnalysis;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Logging;

[ExcludeFromCodeCoverage]
public class ConsoleLogger : ILogger{
    #region Constructors

    public ConsoleLogger(){
        this.IsInitialised = true;
    }

    #endregion

    #region Properties

    public Boolean IsInitialised{ get; set; }

    #endregion

    #region Methods

    internal void Log(LogMessage logMessageModel)
    {
        List<LogMessage> logMessageModels = new List<LogMessage>{
                                                                    logMessageModel
                                                                };
        this.Log(logMessageModels);
    }

    internal void Log(List<LogMessage> logMessageModels)
    {
        foreach (LogMessage item in logMessageModels)
        {
            Console.WriteLine($"AppLog|{item.EntryDateTime}|{item.LogLevel}|{item.Message}");
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