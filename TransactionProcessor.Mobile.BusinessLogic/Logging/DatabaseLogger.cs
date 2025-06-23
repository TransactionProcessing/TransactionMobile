using System.Diagnostics.CodeAnalysis;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Logging;

using LogMessage = Models.LogMessage;

[ExcludeFromCodeCoverage]
public class DatabaseLogger : ILogger{
    #region Fields

    private readonly IApplicationCache ApplicationCache;

    private readonly IDatabaseContext DatabaseContext;

    #endregion

    #region Constructors

    public DatabaseLogger(IDatabaseContext databaseContext, IApplicationCache applicationCache){
        this.DatabaseContext = databaseContext;
        this.ApplicationCache = applicationCache;
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
        List<Database.LogMessage> logMessages = new List<Database.LogMessage>();
        foreach (LogMessage item in logMessageModels)
        {
            logMessages.Add(new Database.LogMessage
                            {
                                EntryDateTime = item.EntryDateTime,
                                LogLevel = item.LogLevel.ToString(),
                                Message = item.Message
                            });
        }

        this.DatabaseContext.InsertLogMessages(logMessages);
    }

    public void LogCritical(String message, Exception exception) => this.Log(LogMessage.CreateFatalLogMessages(message, exception));

    public void LogDebug(String message) => this.Log(LogMessage.CreateDebugLogMessage(message));

    public void LogError(String message, Exception exception) => this.Log(LogMessage.CreateErrorLogMessages(message, exception));
    public void LogInformation(String message) => this.Log(LogMessage.CreateInformationLogMessage(message));

    public void LogTrace(String message) => this.Log(LogMessage.CreateTraceLogMessage(message));

    public void LogWarning(String message) => this.Log(LogMessage.CreateWarningLogMessage(message));

    #endregion
}