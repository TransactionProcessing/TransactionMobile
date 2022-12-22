using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Logging
{
    using Database;
    using MetroLog;
    using Microsoft.Extensions.Logging;

    public interface ILoggerService
    {
        void LogCritical(String message,Exception exception);
        void LogError(String message, Exception exception);
        void LogWarning(String message, Exception exception);
        void LogWarning(String message);
        void LogInformation(String message);
        void LogDebug(String message);
        void LogTrace(String message);
    }

    // TODO: Database logger

    public class MetroLogLoggerService : ILoggerService
    {
        private readonly ILogger<Application> Logger;

        public MetroLogLoggerService(ILogger<Application> logger) {
            this.Logger = logger;
        }

        public void LogCritical(String message, Exception exception) {
            this.Logger.LogCritical(exception, message);
        }

        public void LogError(String message, Exception exception) {
            this.Logger.LogError(exception, message);
        }

        public void LogWarning(String message) {
            this.Logger.LogWarning(message);
        }

        public void LogWarning(String message, Exception exception)
        {
            this.Logger.LogWarning(exception,message);
        }

        public void LogInformation(String message) {
            this.Logger.LogInformation(message);
        }

        public void LogDebug(String message) {
            this.Logger.LogDebug(message);
        }

        public void LogTrace(String message) {
            this.Logger.LogTrace(message);
        }
    }
}
