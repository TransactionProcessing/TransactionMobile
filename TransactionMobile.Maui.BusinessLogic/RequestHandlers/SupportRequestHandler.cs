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
        private readonly IConfigurationService ConfigurationService;
        private readonly IDatabaseContext DatabaseContext;
        public SupportRequestHandler(IConfigurationService configurationService,
                                     IDatabaseContext databaseContext)
        {
            this.ConfigurationService = configurationService;
            this.DatabaseContext = databaseContext;
        }

        public async Task<Boolean> Handle(UploadLogsRequest request, CancellationToken cancellationToken)
        {
            while (true)
            {
                var logEntries = await this.DatabaseContext.GetLogMessages(10); // TODO: Configurable batch size

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

                await this.ConfigurationService.PostDiagnosticLogs(request.DeviceIdentifier, logMessageModels, CancellationToken.None);

                // Clear the logs that have been uploaded
                await this.DatabaseContext.RemoveUploadedMessages(logEntries);
            }

            return true;
        }
    }
}
