using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Services.DataTransferObjects;

[ExcludeFromCodeCoverage]
public class ConfigurationResponse
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public string DeviceIdentifier { get; set; }

    public bool EnableAutoUpdates { get; set; }

    public List<HostAddress> HostAddresses { get; set; }

    public string Id { get; set; }

    public LoggingLevel LogLevel { get; set; }

    public ApplicationCentreConfiguration ApplicationCentreConfiguration { get; set; }
}