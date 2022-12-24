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
    using Services;
    using LogMessage = Models.LogMessage;

    public interface ILoggerService
    {
        Task LogCritical(String message,Exception exception);
        Task LogError(String message, Exception exception);
        Task LogWarning(String message);
        Task LogInformation(String message);
        Task LogDebug(String message);
        Task LogTrace(String message);
    }

    // TODO: Database logger

    public class MetroLogLoggerService : ILoggerService
    {
        private readonly IConfigurationService ConfigurationService;

        public MetroLogLoggerService(IConfigurationService configurationService) {
            this.ConfigurationService = configurationService;
        }

        private async Task SendLogMessages(List<LogMessage> logMessages, CancellationToken cancellationToken) {
            await this.ConfigurationService.PostDiagnosticLogs("", logMessages, cancellationToken);
        }

        public async Task LogCritical(String message, Exception exception) {
            List<LogMessage> logMessageModels = Models.LogMessage.CreateFatalLogMessages(message, exception);
            await this.SendLogMessages(logMessageModels,CancellationToken.None);
        }

        public async Task LogError(String message, Exception exception) {
            var logMessageModels = Models.LogMessage.CreateErrorLogMessages(message, exception);
            await this.SendLogMessages(logMessageModels, CancellationToken.None);
        }

        public async Task LogWarning(String message) {
            var logMessageModel = Models.LogMessage.CreateWarningLogMessage(message);
            await this.SendLogMessages(new List<LogMessage>{logMessageModel}, CancellationToken.None);
        }

        public async Task LogInformation(String message) {
            var logMessageModel = Models.LogMessage.CreateInformationLogMessage(message);
            await this.SendLogMessages(new List<LogMessage> { logMessageModel }, CancellationToken.None);
        }

        public async Task LogDebug(String message) {
            var logMessageModel = Models.LogMessage.CreateDebugLogMessage(message);
            await this.SendLogMessages(new List<LogMessage> { logMessageModel }, CancellationToken.None);
        }

        public async Task LogTrace(String message) {
            var logMessageModel = Models.LogMessage.CreateTraceLogMessage(message);
            await this.SendLogMessages(new List<LogMessage> { logMessageModel }, CancellationToken.None);
        }
    }
}
