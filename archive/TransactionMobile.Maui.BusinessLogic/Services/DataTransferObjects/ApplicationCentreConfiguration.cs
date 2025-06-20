using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Services.DataTransferObjects;

[ExcludeFromCodeCoverage]
public class ApplicationCentreConfiguration
{
    public string ApplicationId { get; set; }
    public string AndroidKey { get; set; }
    public string IosKey { get; set; }
    public string MacosKey { get; set; }
    public string WindowsKey { get; set; }
}