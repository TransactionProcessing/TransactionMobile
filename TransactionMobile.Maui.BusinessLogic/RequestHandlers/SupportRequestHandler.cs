using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.BusinessLogic.Requests;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;

namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers
{
    public class SupportRequestHandler : IRequestHandler<UploadLogsRequest, Boolean>
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
            while (true)
            {
                List<LogMessage> logEntries = await this.DatabaseContext.GetLogMessages(10); // TODO: Configurable batch size

                if (logEntries.Any() == false)
                {
                    break;
                }

                // TODO: Translate log messages
                List<Models.LogMessage> logMessageModels = new List<Models.LogMessage>();

                logEntries.ForEach(l => logMessageModels.Add(new Models.LogMessage
                {
                    LogLevel = l.LogLevel,
                    Message = l.Message,
                    EntryDateTime = l.EntryDateTime,
                    Id = l.Id
                }));
                Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
                IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);
                await configurationService.PostDiagnosticLogs(request.DeviceIdentifier, logMessageModels, CancellationToken.None);

                // Clear the logs that have been uploaded
                await this.DatabaseContext.RemoveUploadedMessages(logEntries);
            }

            return true;
        }
    }
}
