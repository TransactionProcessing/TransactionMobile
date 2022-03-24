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
        private readonly IMemoryCacheService MemoryCacheService;
        public SupportRequestHandler(Func<Boolean, IConfigurationService> configurationServiceResolver,
                                     IDatabaseContext databaseContext,
                                     IMemoryCacheService memoryCacheService)
        {
            this.ConfigurationServiceResolver = configurationServiceResolver;
            this.DatabaseContext = databaseContext;
            this.MemoryCacheService = memoryCacheService;
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
                this.MemoryCacheService.TryGetValue("UseTrainingMode", out Boolean useTrainingMode);
                var configurationService = this.ConfigurationServiceResolver(useTrainingMode);
                await configurationService.PostDiagnosticLogs(request.DeviceIdentifier, logMessageModels, CancellationToken.None);

                // Clear the logs that have been uploaded
                await this.DatabaseContext.RemoveUploadedMessages(logEntries);
            }

            return true;
        }
    }
}
