using Sentry;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.UIServices;

public class SentryService : ISentryService
{
    public void InitializeSentry(String dsn)
    {
        if (String.IsNullOrWhiteSpace(dsn))
        {
            return;
        }

        SentrySdk.Init(o =>
        {
            o.Dsn = dsn;
        });
    }
}
