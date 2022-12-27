using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Logging
{
    using Database;
    using Services;

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

    public static class Logger
    {
        #region Fields

        /// <summary>
        /// The logger object
        /// </summary>
        private static List<ILogger> LoggerObjects;

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
            if (loggerObject == null) {
                throw new ArgumentNullException(nameof(loggerObject));
            }

            if (Logger.LoggerObjects == null) {
                Logger.LoggerObjects = new List<ILogger>();
            }
            Logger.LoggerObjects.Add(loggerObject);

            Logger.IsInitialised = true;
        }

        /// <summary>
        /// Logs the critical.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogCritical(String message, Exception exception)
        {
            foreach (ILogger loggerObject in Logger.LoggerObjects) {
                loggerObject.LogCritical(message, exception);
            }
            
        }

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogDebug(String message)
        {
            foreach (ILogger loggerObject in Logger.LoggerObjects)
            {
                loggerObject.LogDebug(message);
            }
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void LogError(String message, Exception exception)
        {
            foreach (ILogger loggerObject in Logger.LoggerObjects) {
                loggerObject.LogError(message, exception);
            }
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogInformation(String message)
        {
            foreach (ILogger loggerObject in Logger.LoggerObjects)
            {
                loggerObject.LogInformation(message);
            }
        }

        /// <summary>
        /// Logs the trace.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogTrace(String message)
        {
            foreach (ILogger loggerObject in Logger.LoggerObjects) {
                loggerObject.LogTrace(message);
            }
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogWarning(String message)
        {
            foreach (ILogger loggerObject in Logger.LoggerObjects)
            {
                loggerObject.LogWarning(message);
            }
        }

        #endregion
    }
    public class NullLogger : ILogger
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialised; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsInitialised { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        public void Initialise()
        {
            this.IsInitialised = true;
        }

        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns></returns>
        public static NullLogger Instance
        {
            get
            {
                return new NullLogger();
            }
        }

        /// <summary>
        /// Logs the critical.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void LogCritical(String message, Exception exception)
        {
        }

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogDebug(String message)
        {
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void LogError(String message, Exception exception)
        {
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInformation(String message)
        {
        }

        /// <summary>
        /// Logs the trace.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogTrace(String message)
        {
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarning(String message)
        {
        }

        #endregion
    }

    public class DatabaseLogger : ILogger
    {
        private readonly IDatabaseContext DatabaseContext;

        private readonly IApplicationCache ApplicationCache;

        public DatabaseLogger(IDatabaseContext databaseContext, IApplicationCache applicationCache)
        {
            this.DatabaseContext = databaseContext;
            this.ApplicationCache = applicationCache;
            this.IsInitialised = true;
        }

        public bool IsInitialised { get; set; }

        public void LogCritical(String message,Exception exception)
        {
            var logMessageModels = Models.LogMessage.CreateFatalLogMessages(message, exception);
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

        public void LogError(String message, Exception exception)
        {
            var logMessageModels = Models.LogMessage.CreateErrorLogMessages(message, exception);
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

    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            this.IsInitialised = true;
        }

        public bool IsInitialised { get; set; }

        public void LogCritical(String message, Exception exception)
        {
            var logMessageModels = Models.LogMessage.CreateFatalLogMessages(message, exception);
            var logMessages = new List<Database.LogMessage>();
            foreach (var item in logMessageModels)
            {
                Console.WriteLine($"AppLog|{item.EntryDateTime}|{item.LogLevel}|{item.Message}");
            }
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
            Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
        }

        public void LogError(String message, Exception exception)
        {
            var logMessageModels = Models.LogMessage.CreateErrorLogMessages(message, exception);
            var logMessages = new List<Database.LogMessage>();
            foreach (var item in logMessageModels)
            {
                Console.WriteLine($"AppLog|{item.EntryDateTime}|{item.LogLevel}|{item.Message}");
            }
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
            Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
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
            Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
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
            Console.WriteLine($"AppLog|{logMessage.EntryDateTime}|{logMessage.LogLevel}|{logMessage.Message}");
        }
    }
}
