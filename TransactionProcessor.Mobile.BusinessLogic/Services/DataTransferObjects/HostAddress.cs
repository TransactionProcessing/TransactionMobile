using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Services.DataTransferObjects;

[ExcludeFromCodeCoverage]
public class HostAddress
{
    public ServiceType ServiceType { get; set; }

    public string Uri { get; set; }
}