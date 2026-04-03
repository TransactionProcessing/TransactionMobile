using Browser = Microsoft.Maui.ApplicationModel.Browser;
using OperationCanceledException = System.OperationCanceledException;
using Microsoft.Maui.ApplicationModel;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
#if ANDROID
using Android.Content;
using Android.Provider;
using AndroidX.Core.Content;
using JavaFile = Java.IO.File;
using MauiFileProvider = AndroidX.Core.Content.FileProvider;
#endif

namespace TransactionProcessor.Mobile.UIServices;

public class ApplicationUpdateLauncherService : IApplicationUpdateLauncherService
{
    private const Int32 DownloadBufferSize = 81920;
    private const String AndroidPackageArchiveMimeType = "application/vnd.android.package-archive";
    private readonly IHttpClientFactory HttpClientFactory;

    public ApplicationUpdateLauncherService(IHttpClientFactory httpClientFactory)
    {
        this.HttpClientFactory = httpClientFactory;
    }

    public async Task LaunchUpdateAsync(String downloadUri,
                                        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(downloadUri);

        if (!Uri.TryCreate(downloadUri, UriKind.Absolute, out Uri? updateUri))
        {
            throw new ArgumentException("An application update is required, but the download address is invalid.", nameof(downloadUri));
        }

#if ANDROID
        if (OperatingSystem.IsAndroid())
        {
            await this.LaunchAndroidUpdateAsync(updateUri, cancellationToken);
            return;
        }
#endif

        if (updateUri.Scheme == Uri.UriSchemeHttp || updateUri.Scheme == Uri.UriSchemeHttps)
        {
            await Browser.Default.OpenAsync(updateUri, BrowserLaunchMode.External);
            return;
        }

        if (!await Launcher.Default.CanOpenAsync(updateUri))
        {
            throw new InvalidOperationException($"The application update address '{downloadUri}' cannot be opened on this device.");
        }

        await Launcher.Default.OpenAsync(updateUri);
    }

#if ANDROID
    private async Task LaunchAndroidUpdateAsync(Uri updateUri,
                                                CancellationToken cancellationToken)
    {
        this.ValidateAndroidUpdateUri(updateUri);

        try
        {
            Context context = Platform.AppContext ?? Android.App.Application.Context;
            this.EnsureInstallPermission(context);

            String updateFilePath = await this.DownloadUpdatePackageAsync(updateUri, cancellationToken);
            JavaFile updateFile = new(updateFilePath);

            if (!updateFile.Exists())
            {
                throw new ApplicationException("The update package could not be prepared for installation.");
            }

            Android.Net.Uri installerUri = MauiFileProvider.GetUriForFile(context, $"{context.PackageName}.fileprovider", updateFile);
            Intent installIntent = this.CreateInstallIntent(installerUri);
            installIntent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);
            installIntent.PutExtra(Intent.ExtraReturnResult, false);

            if (installIntent.ResolveActivity(context.PackageManager) is null)
            {
                throw new ApplicationException("No installer is available on this device to open the update package.");
            }

            context.StartActivity(installIntent);
        }
        catch (Exception ex) when (ex is not ApplicationException && ex is not ArgumentException && ex is not OperationCanceledException)
        {
            throw new ApplicationException("Unable to start the application update installer.", ex);
        }
    }

    private void ValidateAndroidUpdateUri(Uri updateUri)
    {
        if (updateUri.Scheme != Uri.UriSchemeHttp && updateUri.Scheme != Uri.UriSchemeHttps)
        {
            throw new ArgumentException("Android updates require a valid HTTP or HTTPS download address.", nameof(updateUri));
        }
    }

    private void EnsureInstallPermission(Context context)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(26) || context.PackageManager?.CanRequestPackageInstalls() is true)
        {
            return;
        }

        Intent settingsIntent = new(Settings.ActionManageUnknownAppSources);
        settingsIntent.SetData(Android.Net.Uri.Parse($"package:{context.PackageName}"));
        settingsIntent.AddFlags(ActivityFlags.NewTask);
        context.StartActivity(settingsIntent);

        throw new ApplicationException("Allow installs from this app, then retry the update.");
    }

    private Intent CreateInstallIntent(Android.Net.Uri installerUri)
    {
        Intent installIntent = OperatingSystem.IsAndroidVersionAtLeast(29)
                                   ? new Intent(Intent.ActionView)
                                   : new Intent(Intent.ActionInstallPackage);

        if (OperatingSystem.IsAndroidVersionAtLeast(29))
        {
            installIntent.SetDataAndType(installerUri, AndroidPackageArchiveMimeType);
        }
        else
        {
            installIntent.SetData(installerUri);
        }

        return installIntent;
    }

    private async Task<String> DownloadUpdatePackageAsync(Uri updateUri,
                                                          CancellationToken cancellationToken)
    {
        String updatesDirectory = Path.Combine(FileSystem.CacheDirectory, "updates");
        Directory.CreateDirectory(updatesDirectory);

        String fileName = this.GetUpdateFileName(updateUri);
        String updateFilePath = Path.Combine(updatesDirectory, fileName);

        if (System.IO.File.Exists(updateFilePath))
        {
            System.IO.File.Delete(updateFilePath);
        }

        HttpClient httpClient = this.HttpClientFactory.CreateClient("default");
        using HttpResponseMessage response = await httpClient.GetAsync(updateUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"The update package could not be downloaded ({(Int32)response.StatusCode} {response.ReasonPhrase}).");
        }

        await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using FileStream destinationStream = new(updateFilePath, FileMode.Create, FileAccess.Write, FileShare.None, DownloadBufferSize, true);
        await responseStream.CopyToAsync(destinationStream, cancellationToken);

        return updateFilePath;
    }

    private String GetUpdateFileName(Uri updateUri)
    {
        String fileName = Path.GetFileName(updateUri.LocalPath);

        if (String.IsNullOrWhiteSpace(fileName))
        {
            return "transactionprocessor_mobile_update.apk";
        }

        foreach (Char invalidCharacter in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidCharacter, '_');
        }

        if (!fileName.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
        {
            fileName = $"{fileName}.apk";
        }

        return fileName;
    }
#endif
}
