using System.Diagnostics.CodeAnalysis;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services.TrainingModeServices;

[ExcludeFromCodeCoverage]
public class TrainingConfigurationService : IConfigurationService
{
    public async Task<Result<Configuration>> GetConfiguration(String deviceIdentifier,
                                                              CancellationToken cancellationToken) {
        return Result.Success(new Configuration {
                                                    ClientId = "dummyClientId",
                                                    ClientSecret = "dummyClientSecret",
                                                    EnableAutoUpdates = false,
                                                    ShowDebugMessages = true,
                                                    TransactionProcessorUri = "http://localhost:5001",
                                                    EstateReportingUri = "http://localhost:5006",
                                                    LogLevel = LogLevel.Debug,
                                                    SecurityServiceUri = "http://localhost:5001",
                                                    TransactionProcessorAclUri = "http://localhost:5003"
                                                });
    }

    public async Task PostDiagnosticLogs(String deviceIdentifier,
                                         List<LogMessage> logMessages,
                                         CancellationToken cancellationToken) {
        // Do nothing
    }
}