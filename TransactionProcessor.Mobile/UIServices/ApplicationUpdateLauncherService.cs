using Microsoft.Maui.ApplicationModel;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.UIServices;

public class ApplicationUpdateLauncherService : IApplicationUpdateLauncherService
{
    public async Task LaunchUpdateAsync(String downloadUri,
                                        CancellationToken cancellationToken = default)
    {
        if (String.IsNullOrWhiteSpace(downloadUri))
        {
            throw new ArgumentException("A download URI is required to launch an application update.", nameof(downloadUri));
        }

        Uri updateUri = new(downloadUri, UriKind.Absolute);

        if (await Launcher.Default.CanOpenAsync(updateUri) == false)
        {
            throw new InvalidOperationException($"The application update address '{downloadUri}' cannot be opened on this device.");
        }

        await Launcher.Default.OpenAsync(updateUri);
    }
}
