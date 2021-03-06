
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Logger;
using TransactionMobile.Maui.Database;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    public class DatabaseLogger : ILogger
    {
        private readonly IDatabaseContext DatabaseContext;
        public DatabaseLogger(IDatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
            this.IsInitialised = true;    
        }

        public bool IsInitialised { get; set; }

        public void LogCritical(Exception exception)
        {
            var logMessageModels = Models.LogMessage.CreateFatalLogMessages(exception);
            var logMessages = new List<Database.LogMessage>();
            foreach (var item in logMessageModels)
            {
                logMessages.Add(new LogMessage
                {
                    EntryDateTime = item.EntryDateTime,
                    LogLevel = item.LogLevel.ToString(),
                    Message = item.Message
                });
            }
            this.DatabaseContext.InsertLogMessages(logMessages);
        }

        public void LogDebug(string message)
        {
            var logMessageModel = Models.LogMessage.CreateDebugLogMessage(message);
            var logMessage = new LogMessage
            {
                EntryDateTime = logMessageModel.EntryDateTime,
                LogLevel = logMessageModel.LogLevel.ToString(),
                Message = logMessageModel.Message
            };
            this.DatabaseContext.InsertLogMessage(logMessage);
        }

        public void LogError(Exception exception)
        {
            var logMessageModels = Models.LogMessage.CreateErrorLogMessages(exception);
            var logMessages = new List<Database.LogMessage>();
            foreach (var item in logMessageModels)
            {
                logMessages.Add(new LogMessage
                {
                    EntryDateTime = item.EntryDateTime,
                    LogLevel = item.LogLevel.ToString(),
                    Message = item.Message
                });
            }
            this.DatabaseContext.InsertLogMessages(logMessages);
        }

        public void LogInformation(string message)
        {
            var logMessageModel = Models.LogMessage.CreateInformationLogMessage(message);
            var logMessage = new LogMessage
            {
                EntryDateTime = logMessageModel.EntryDateTime,
                LogLevel = logMessageModel.LogLevel.ToString(),
                Message = logMessageModel.Message
            };
            this.DatabaseContext.InsertLogMessage(logMessage);
        }

        public void LogTrace(string message)
        {
            var logMessageModel = Models.LogMessage.CreateTraceLogMessage(message);
            var logMessage = new LogMessage
            {
                EntryDateTime = logMessageModel.EntryDateTime,
                LogLevel = logMessageModel.LogLevel.ToString(),
                Message = logMessageModel.Message
            };
            this.DatabaseContext.InsertLogMessage(logMessage);
        }

        public void LogWarning(string message)
        {
            var logMessageModel = Models.LogMessage.CreateWarningLogMessage(message);
            var logMessage = new LogMessage
            {
                EntryDateTime = logMessageModel.EntryDateTime,
                LogLevel = logMessageModel.LogLevel.ToString(),
                Message = logMessageModel.Message
            };
            this.DatabaseContext.InsertLogMessage(logMessage);
        }
    }
}
