using Android.App;
using Android.Runtime;

namespace TransactionMobile.Maui;

[Application(AllowBackup = false, Debuggable = true, UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
