namespace TransactionMobile.Maui.BusinessLogic.Services.DummyServices
{
    using Models;

    public class DummyConfigurationService : IConfigurationService
    {
        #region Methods

        public async Task<Configuration> GetConfiguration(String deviceIdentifier,
                                                          CancellationToken cancellationToken)
        {
            return new Configuration
                   {
                       ClientId = "dummyClientId",
                       ClientSecret = "dummyClientSecret",
                       EnableAutoUpdates = false,
                       EstateManagementUri = "http://localhost:5000",
                       EstateReportingUri = "http://localhost:5006",
                       LogLevel = LogLevel.Debug,
                       SecurityServiceUri = "http://localhost:5001",
                       TransactionProcessorAclUri = "http://localhost:5003"
                   };
        }

        public async Task PostDiagnosticLogs(String deviceIdentifier,
                                             List<LogMessage> logMessages,
                                             CancellationToken cancellationToken)
        {
        }

        #endregion
    }
}