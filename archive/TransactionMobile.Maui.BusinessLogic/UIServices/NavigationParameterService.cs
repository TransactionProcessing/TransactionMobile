using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.UIServices;

[ExcludeFromCodeCoverage]
public class NavigationParameterService : INavigationParameterService
{
    private IDictionary<String, Object> parameters;
    public void SetParameters(IDictionary<String, Object> parameters)
    {
        this.parameters = parameters;
    }
    public IDictionary<String, Object> GetParameters()
    {
        return this.parameters;
    }
}