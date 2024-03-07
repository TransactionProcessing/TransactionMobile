namespace TransactionMobile.Maui.BusinessLogic.Services.TrainingModeServices;

using Models;
using RequestHandlers;
using System.Diagnostics.CodeAnalysis;
using Common;
using SimpleResults;

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
                                                    EstateManagementUri = "http://localhost:5000",
                                                    EstateReportingUri = "http://localhost:5006",
                                                    LogLevel = LogLevel.Debug,
                                                    SecurityServiceUri = "http://localhost:5001",
                                                    TransactionProcessorAclUri = "http://localhost:5003",
                                                    AppCenterConfig = new AppCenterConfiguration {
                                                                                                     AndroidKey = String.Empty,
                                                                                                     MacOSKey = String.Empty,
                                                                                                     WindowsKey = String.Empty,
                                                                                                     iOSKey = String.Empty
                                                                                                 }
                                                });
    }

    public async Task PostDiagnosticLogs(String deviceIdentifier,
                                         List<LogMessage> logMessages,
                                         CancellationToken cancellationToken) {
            
    }
}