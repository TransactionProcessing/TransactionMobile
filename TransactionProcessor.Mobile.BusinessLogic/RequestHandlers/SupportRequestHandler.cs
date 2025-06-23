using MediatR;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers
{
    public class SupportRequestHandler : IRequestHandler<UploadLogsRequest, Boolean>,
                                         IRequestHandler<ViewLogsRequest, List<Models.LogMessage>>
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

        public async Task<Boolean> Handle(UploadLogsRequest request, CancellationToken cancellationToken)
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            while (true)
            {
                List<LogMessage> logEntries = await this.DatabaseContext.GetLogMessages(10, useTrainingMode); // TODO: Configurable batch size

                if (logEntries.Any() == false)
                {
                    break;
                }

                List<Models.LogMessage> logMessageModels = new List<Models.LogMessage>();

                logEntries.ForEach(l => logMessageModels.Add(new Models.LogMessage
                {
                    LogLevel = Enum.Parse<Models.LogLevel>(l.LogLevel),
                    LogLevelString = l.LogLevel,
                    Message = l.Message,
                    EntryDateTime = l.EntryDateTime,
                    Id = l.Id
                }));

                IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);
                await configurationService.PostDiagnosticLogs(request.DeviceIdentifier, logMessageModels, CancellationToken.None);

                // Clear the logs that have been uploaded
                await this.DatabaseContext.RemoveUploadedMessages(logEntries);
            }

            return true;
        }

        public async Task<List<Models.LogMessage>> Handle(ViewLogsRequest request,
                                                    CancellationToken cancellationToken) {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            List<LogMessage> logEntries = await this.DatabaseContext.GetLogMessages(50, useTrainingMode); // TODO: Configurable batch size

            List<Models.LogMessage> logMessageModels = new List<Models.LogMessage>();

            logEntries.ForEach(l => logMessageModels.Add(new Models.LogMessage {
                                                                                   LogLevel = Enum.Parse<Models.LogLevel>(l.LogLevel),
                                                                                   LogLevelString = l.LogLevel,
                                                                                   Message = l.Message,
                                                                                   EntryDateTime = l.EntryDateTime,
                                                                                   Id = l.Id
                                                                               }));

            return logMessageModels.OrderByDescending(l => l.EntryDateTime).ToList();
        }
    }
}
