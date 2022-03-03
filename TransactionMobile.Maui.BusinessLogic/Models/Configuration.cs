namespace TransactionMobile.Maui.BusinessLogic.Models;

public class Configuration
{
    public String ClientId { get; set; }

    public String ClientSecret { get; set; }

    public String SecurityServiceUrl { get; set; }
        
    public String TransactionProcessorAclUrl { get; set; }

    public String EstateManagementUrl { get; set; }

    public String EstateReportingUrl { get; set; }

    public LogLevel LogLevel { get; set; }

    public Boolean EnableAutoUpdates { get; set; }
}