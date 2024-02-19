namespace TransactionMobile.Maui.BusinessLogic.Logging;

using System.Diagnostics.CodeAnalysis;
using Database;
using Services;
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

    public void LogCritical(String message, Exception exception){
        var logMessageModels = LogMessage.CreateFatalLogMessages(message, exception);
        var logMessages = new List<Database.LogMessage>();
        foreach (var item in logMessageModels){
            logMessages.Add(new Database.LogMessage{
                                                       EntryDateTime = item.EntryDateTime,
                                                       LogLevel = item.LogLevel.ToString(),
                                                       Message = item.Message
                                                   });
        }

        this.DatabaseContext.InsertLogMessages(logMessages);
    }

    public void LogDebug(String message){
        var logMessageModel = LogMessage.CreateDebugLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        this.DatabaseContext.InsertLogMessage(logMessage);
    }

    public void LogError(String message, Exception exception){
        var logMessageModels = LogMessage.CreateErrorLogMessages(message, exception);
        var logMessages = new List<Database.LogMessage>();
        foreach (var item in logMessageModels){
            logMessages.Add(new Database.LogMessage{
                                                       EntryDateTime = item.EntryDateTime,
                                                       LogLevel = item.LogLevel.ToString(),
                                                       Message = item.Message
                                                   });
        }

        this.DatabaseContext.InsertLogMessages(logMessages);
    }

    public void LogInformation(String message){
        var logMessageModel = LogMessage.CreateInformationLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        this.DatabaseContext.InsertLogMessage(logMessage);
    }

    public void LogTrace(String message){
        var logMessageModel = LogMessage.CreateTraceLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        this.DatabaseContext.InsertLogMessage(logMessage);
    }

    public void LogWarning(String message){
        var logMessageModel = LogMessage.CreateWarningLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        this.DatabaseContext.InsertLogMessage(logMessage);
    }

    #endregion
}