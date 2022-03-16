namespace TransactionMobile.Maui.BusinessLogic.Models;

public class Configuration
{
    public String ClientId { get; set; }

    public String ClientSecret { get; set; }

    public String SecurityServiceUri { get; set; }
        
    public String TransactionProcessorAclUri { get; set; }

    public String EstateManagementUri { get; set; }

    public String EstateReportingUri { get; set; }

    public LogLevel LogLevel { get; set; }

    public Boolean EnableAutoUpdates { get; set; }
}