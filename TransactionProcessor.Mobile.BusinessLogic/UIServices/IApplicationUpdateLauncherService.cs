namespace TransactionProcessor.Mobile.BusinessLogic.UIServices;

public interface IApplicationUpdateLauncherService
{
    Task LaunchUpdateAsync(String downloadUri,
                           CancellationToken cancellationToken = default);
}
