using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using LogMessage = TransactionProcessor.Mobile.BusinessLogic.Database.LogMessage;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers
{
    public class SupportRequestHandler : IRequestHandler<SupportCommands.UploadLogsCommand, Result>,
                                         IRequestHandler<SupportQueries.ViewLogsQuery, Result<List<Models.LogMessage>>>
    {
        private readonly Func<Boolean, IConfigurationService> ConfigurationServiceResolver;
        private readonly IDatabaseContext DatabaseContext;

        private readonly IApplicationCache ApplicationCache;

        public SupportRequestHandler(Func<Boolean, IConfigurationService> configurationServiceResolver,
                                     IDatabaseContext databaseContext,
                                     IApplicationCache applicationCache)
        {
            this.ConfigurationServiceResolver = configurationServiceResolver;
            this.DatabaseContext = databaseContext;
            this.ApplicationCache = applicationCache;
        }

    public async Task<Result> Handle(SupportCommands.UploadLogsCommand request, CancellationToken cancellationToken)
    {
        Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
        Configuration configuration = this.ApplicationCache.GetConfiguration();
        if (configuration == null)
        {
            return Result.Failure("Configuration is not available.");
        }

        while (true) {
            cancellationToken.ThrowIfCancellationRequested();
            IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);

            List<LogMessage> logEntries = await this.DatabaseContext.GetLogMessages(configuration.LogMessageBatchSize.GetValueOrDefault(10), useTrainingMode);

            if (logEntries.Any() == false) {
                break;
            }

            Result<List<Models.LogMessage>> logMessageModelsResult = this.TryBuildLogMessageModels(logEntries);
            if (logMessageModelsResult.IsFailed)
            {
                return Result.Failure(logMessageModelsResult.Errors.FirstOrDefault() ?? "Unable to map log messages.");
            }

            Result result = await configurationService.PostDiagnosticLogs(request.DeviceIdentifier, logMessageModelsResult.Data, cancellationToken);

            if (result.IsFailed) {
                // We have had a failure posting the logs so we will stop trying to upload any more logs
                return result;
                }

                // Clear the logs that have been uploaded
                await this.DatabaseContext.RemoveUploadedMessages(logEntries);
                
            }

            return Result.Success();
        }

        public async Task<Result<List<Models.LogMessage>>> Handle(SupportQueries.ViewLogsQuery request,
                                                    CancellationToken cancellationToken) {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            List<LogMessage> logEntries = await this.DatabaseContext.GetLogMessages(50, useTrainingMode);

            Result<List<Models.LogMessage>> logMessageModelsResult = this.TryBuildLogMessageModels(logEntries);
            if (logMessageModelsResult.IsFailed)
            {
                return Result.Failure(logMessageModelsResult.Errors.FirstOrDefault() ?? "Unable to map log messages.");
            }

            return Result.Success(logMessageModelsResult.Data.OrderByDescending(l => l.EntryDateTime).ToList());
        }

        private Result<List<Models.LogMessage>> TryBuildLogMessageModels(List<LogMessage> logEntries)
        {
            List<Models.LogMessage> logMessageModels = new();

            foreach (LogMessage logEntry in logEntries)
            {
                if (Enum.TryParse<Models.LogLevel>(logEntry.LogLevel, ignoreCase: true, out Models.LogLevel logLevel) == false)
                {
                    return Result.Failure($"Invalid log level '{logEntry.LogLevel}'.");
                }

                logMessageModels.Add(new Models.LogMessage
                {
                    LogLevel = logLevel,
                    LogLevelString = logEntry.LogLevel,
                    Message = logEntry.Message,
                    EntryDateTime = logEntry.EntryDateTime,
                    Id = logEntry.Id
                });
            }

            return Result.Success(logMessageModels);
        }
    }
}
