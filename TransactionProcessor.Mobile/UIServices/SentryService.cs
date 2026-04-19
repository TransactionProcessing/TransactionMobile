using Sentry;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.UIServices;

public class SentryService : ISentryService
{
    private readonly IApplicationInfoService ApplicationInfoService;
    private Boolean isInitialized;

    public SentryService(IApplicationInfoService applicationInfoService) {
        this.ApplicationInfoService = applicationInfoService;
    }

    public void InitializeSentry(String dsn)
    {
        if (this.isInitialized || String.IsNullOrWhiteSpace(dsn))
        {
            return;
        }

        SentrySdk.Init(o =>
        {
            o.Dsn = dsn;
#if ANDROID || IOS || MACCATALYST
            o.Native.AttachScreenshot = true;
#endif
            o.SendDefaultPii = true;
            o.Release = this.ApplicationInfoService.VersionString;
        });

        this.isInitialized = true;
    }
}
