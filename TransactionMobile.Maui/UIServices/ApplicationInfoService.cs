namespace TransactionMobile.Maui.UIServices;

using BusinessLogic.UIServices;

public class ApplicationInfoService : IApplicationInfoService
{
    public String BuildString => AppInfo.BuildString;

    public String ApplicationName => AppInfo.Name;

    public String PackageName => AppInfo.PackageName;

    public String Theme => AppInfo.RequestedTheme.ToString();

    public Version Version => AppInfo.Version;

    public String VersionString => "1.0.0";//AppInfo.VersionString;
}