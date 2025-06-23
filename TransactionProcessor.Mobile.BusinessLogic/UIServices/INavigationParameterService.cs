namespace TransactionProcessor.Mobile.BusinessLogic.UIServices;

public interface INavigationParameterService
{
    void SetParameters(IDictionary<String, Object> parameters);
    IDictionary<String, Object> GetParameters();
}