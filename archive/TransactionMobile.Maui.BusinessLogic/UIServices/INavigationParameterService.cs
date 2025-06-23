namespace TransactionMobile.Maui.UIServices;

public interface INavigationParameterService
{
    void SetParameters(IDictionary<String, Object> parameters);
    IDictionary<String, Object> GetParameters();
}