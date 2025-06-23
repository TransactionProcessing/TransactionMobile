namespace TransactionMobile.Maui.BusinessLogic.UIServices;

public interface IApplicationInfoService
{
    String BuildString { get; }

    String ApplicationName { get; }

    String PackageName { get; }

    String Theme { get; }

    Version Version { get; }

    String VersionString { get; }
}