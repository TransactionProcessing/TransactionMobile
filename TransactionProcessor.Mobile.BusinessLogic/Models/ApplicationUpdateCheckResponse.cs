using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class ApplicationUpdateCheckResponse
{
    public Boolean UpdateRequired { get; set; }

    public String? DownloadUri { get; set; }

    public String? LatestVersion { get; set; }

    public String? Message { get; set; }
}
