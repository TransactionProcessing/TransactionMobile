using Sentry;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.UIServices;

public class SentryService : ISentryService
{
    private Boolean isInitialized;

    public void InitializeSentry(String dsn)
    {
        if (this.isInitialized || String.IsNullOrWhiteSpace(dsn))
        {
            return;
        }

        SentrySdk.Init(o =>
        {
            o.Dsn = dsn;
        });

        this.isInitialized = true;
    }
}
