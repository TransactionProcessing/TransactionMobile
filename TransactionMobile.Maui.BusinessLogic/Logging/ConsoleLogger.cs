namespace TransactionMobile.Maui.BusinessLogic.Logging;

using System.Diagnostics.CodeAnalysis;
using Models;

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

    public void LogCritical(String message, Exception exception){
        var logMessageModels = LogMessage.CreateFatalLogMessages(message, exception);
        var logMessages = new List<Database.LogMessage>();
        foreach (var item in logMessageModels){
            Console.WriteLine($"AppLog|{item.EntryDateTime}|{item.LogLevel}|{item.Message}");
        }
    }

    public void LogDebug(String message){
        var logMessageModel = LogMessage.CreateDebugLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
    }

    public void LogError(String message, Exception exception){
        var logMessageModels = LogMessage.CreateErrorLogMessages(message, exception);
        var logMessages = new List<Database.LogMessage>();
        foreach (var item in logMessageModels){
            Console.WriteLine($"AppLog|{item.EntryDateTime}|{item.LogLevel}|{item.Message}");
        }
    }

    public void LogInformation(String message){
        var logMessageModel = LogMessage.CreateInformationLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
    }

    public void LogTrace(String message){
        var logMessageModel = LogMessage.CreateTraceLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
    }

    public void LogWarning(String message){
        var logMessageModel = LogMessage.CreateWarningLogMessage(message);
        var logMessage = new Database.LogMessage{
                                                    EntryDateTime = logMessageModel.EntryDateTime,
                                                    LogLevel = logMessageModel.LogLevel.ToString(),
                                                    Message = logMessageModel.Message
                                                };
        Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
    }

    #endregion
}