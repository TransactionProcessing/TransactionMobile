using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class Configuration
{
    public String ClientId { get; set; }

    public String ClientSecret { get; set; }

    public String SecurityServiceUri { get; set; }
        
    public String TransactionProcessorAclUri { get; set; }

    public String TransactionProcessorUri { get; set; }

    public String EstateReportingUri { get; set; }

    public LogLevel LogLevel { get; set; }

    public Boolean EnableAutoUpdates { get; set; }

    public Boolean ShowDebugMessages { get; set; }

    public AppCenterConfiguration AppCenterConfig { get; set; }
}