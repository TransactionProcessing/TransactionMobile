using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Drivers
{
    public enum MobileTestPlatform
    {
        Android,
        Windows,
    }

    public class AppiumDriverWrapper
    {
        private const int AppiumPort = 4723;

        public static MobileTestPlatform MobileTestPlatform;
        public static AppiumDriver Driver;
        private AppiumLocalService? appiumService;
        private Uri? appiumServerUri;
        private readonly List<string> appiumStartupOutput = new();
        private string? appiumLogFilePath;
        private TestAppConfig.AppiumTraceMode appiumTraceMode = TestAppConfig.AppiumTraceMode.Summary;
        private bool ownsAppiumService;

        public string? AppiumLogFilePath => this.appiumLogFilePath;

        public string? ServiceUrl => this.appiumServerUri?.ToString();

        public string[] GetStartupOutputSnapshot() => [.. this.appiumStartupOutput];

        public async Task StartAppAsync()
        {
            TestAppConfig testAppConfig = TestAppConfig.Load(AppiumDriverWrapper.MobileTestPlatform);
            if (testAppConfig.Platform != AppiumDriverWrapper.MobileTestPlatform)
            {
                throw new InvalidOperationException($"UITEST_PLATFORM is '{testAppConfig.Platform}' but the active fixture expects '{AppiumDriverWrapper.MobileTestPlatform}'.");
            }

            this.appiumTraceMode = testAppConfig.AppiumTrace;
            await this.CleanupPreviousSessionAsync().ConfigureAwait(false);
            this.appiumStartupOutput.Clear();
            this.appiumLogFilePath = CreateAppiumLogFilePath(testAppConfig.Platform);
            this.appiumServerUri = null;
            this.ownsAppiumService = false;

            this.TraceStartupConfiguration(testAppConfig);
            Uri? externalAppiumServerUri = await TryGetRunningAppiumServerUriAsync(testAppConfig.AppiumStartupTimeout).ConfigureAwait(false);
            if (externalAppiumServerUri is not null)
            {
                this.appiumServerUri = externalAppiumServerUri;
                this.TraceAppiumSummary($"Reusing existing Appium server at {this.appiumServerUri}.");
            }
            else
            {
                await this.TryStopLikelyAppiumListenersAsync(AppiumPort).ConfigureAwait(false);
                EnsurePortIsAvailable(AppiumPort);

                this.appiumService ??= new AppiumServiceBuilder()
                    .UsingPort(AppiumPort)
                    .WithLogFile(new FileInfo(this.appiumLogFilePath))
                    .WithStartUpTimeOut(testAppConfig.AppiumStartupTimeout)
                    .Build();

                if (this.appiumService.IsRunning == false)
                {
                    this.appiumService.OutputDataReceived += (_, args) => this.TraceAppiumVerbose(args.Data);

                    try
                    {
                        this.appiumService.Start();
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidOperationException(this.BuildStartupFailureMessage(testAppConfig, exception), exception);
                    }
                }

                this.ownsAppiumService = true;
                this.appiumServerUri = this.appiumService.ServiceUrl;
                this.TraceAppiumSummary($"Appium service started at {this.appiumServerUri}");
            }

            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
            {
                AppiumDriverWrapper.SetupAndroidDriver(this.appiumServerUri, testAppConfig);
            }
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                AppiumDriverWrapper.SetupWindowsDriver(this.appiumServerUri, testAppConfig);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported platform '{AppiumDriverWrapper.MobileTestPlatform}'.");
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }

        private static string CreateAppiumLogFilePath(MobileTestPlatform platform)
        {
            string logDirectory = Path.Combine(Path.GetTempPath(), "TransactionProcessor.Mobile.UITests", "Appium");
            Directory.CreateDirectory(logDirectory);

            return Path.Combine(logDirectory, $"appium-{platform}-{DateTime.UtcNow:yyyyMMdd-HHmmssfff}.log");
        }

        private static void EnsurePortIsAvailable(int port)
        {
            try
            {
                using TcpListener listener = new(IPAddress.Loopback, port);
                listener.Start();
            }
            catch (SocketException exception)
            {
                string portConflictDetails = DescribePortConflict(port);
                throw new InvalidOperationException(
                    $"Port {port} is already in use. If Appium is already running manually, stop it before starting the UI tests, or change the harness to use an external Appium server.{Environment.NewLine}{portConflictDetails}",
                    exception);
            }
        }

        private static string DescribePortConflict(int port)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                return $"Port conflict detection is only implemented on Windows for this harness. Check which process is listening on {port}.";
            }

            try
            {
                List<PortOwnerInfo> owners = GetListeningProcesses(port);
                if (owners.Count == 0)
                {
                    return $"No listener could be identified for port {port}, but the bind still failed. Another process may be shutting down or holding the port briefly.";
                }

                var builder = new StringBuilder();
                builder.AppendLine($"Processes listening on {port}:");
                foreach (PortOwnerInfo owner in owners)
                {
                    builder.AppendLine($"- PID {owner.ProcessId}: {owner.ProcessName}");
                    if (string.IsNullOrWhiteSpace(owner.CommandLine) == false)
                    {
                        builder.AppendLine($"  Command line: {owner.CommandLine}");
                    }

                    builder.AppendLine($"  Likely Appium: {owner.IsLikelyAppium}");
                }

                return builder.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"Unable to inspect port ownership for {port}: {ex.Message}";
            }
        }

        private static List<PortOwnerInfo> GetListeningProcesses(int port)
        {
            string output = RunProcess("netstat", "-ano -p tcp");
            var pids = new HashSet<int>();

            foreach (string rawLine in output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string line = rawLine.Trim();
                if (line.StartsWith("TCP", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                string[] parts = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 5)
                {
                    continue;
                }

                string localAddress = parts[1];
                string state = parts[3];
                string pidText = parts[4];

                if (state.Equals("LISTENING", StringComparison.OrdinalIgnoreCase) == false &&
                    state.Equals("LISTEN", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                if (localAddress.EndsWith($":{port}", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                if (int.TryParse(pidText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int pid))
                {
                    pids.Add(pid);
                }
            }

            var owners = new List<PortOwnerInfo>();
            foreach (int pid in pids)
            {
                string processInfo = RunProcess("tasklist", $"/FI \"PID eq {pid}\" /FO CSV /NH");
                string processName = ParseTaskListProcessName(processInfo) ?? $"PID {pid}";
                string? commandLine = TryGetProcessCommandLine(pid);
                owners.Add(new PortOwnerInfo(pid, processName, commandLine, IsLikelyAppiumProcess(processName, commandLine)));
            }

            return owners;
        }

        private async Task TryStopLikelyAppiumListenersAsync(int port)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                return;
            }

            List<PortOwnerInfo> owners = GetListeningProcesses(port);
            List<PortOwnerInfo> appiumOwners = owners.Where(owner => owner.IsLikelyAppium).ToList();
            if (appiumOwners.Count == 0)
            {
                return;
            }

            this.TraceAppiumSummary($"Detected likely stale Appium process(es) on port {port}; attempting to stop them.");

            foreach (PortOwnerInfo owner in appiumOwners)
            {
                this.TraceAppiumSummary($"Stopping PID {owner.ProcessId} ({owner.ProcessName}).");
                if (string.IsNullOrWhiteSpace(owner.CommandLine) == false)
                {
                    this.TraceAppiumVerbose($"Command line: {owner.CommandLine}");
                }

                TryKillProcessTree(owner.ProcessId);
            }

            await WaitForPortToClearAsync(port, TimeSpan.FromSeconds(10)).ConfigureAwait(false);
        }

        private static void TryKillProcessTree(int processId)
        {
            try
            {
                using Process process = new();
                process.StartInfo = new ProcessStartInfo("taskkill", $"/PID {processId} /T /F")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();
                process.WaitForExit(5000);
            }
            catch
            {
                // If the kill fails, the later port check will surface the problem with more context.
            }
        }

        private static async Task WaitForPortToClearAsync(int port, TimeSpan timeout)
        {
            DateTime deadline = DateTime.UtcNow.Add(timeout);
            while (DateTime.UtcNow < deadline)
            {
                if (IsPortFree(port))
                {
                    return;
                }

                await Task.Delay(250).ConfigureAwait(false);
            }
        }

        private static bool IsPortFree(int port)
        {
            try
            {
                using TcpListener listener = new(IPAddress.Loopback, port);
                listener.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string? ParseTaskListProcessName(string output)
        {
            foreach (string rawLine in output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string line = rawLine.Trim();
                if (line.StartsWith("\"", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                string[] parts = line.Split(',');
                if (parts.Length > 0)
                {
                    return parts[0].Trim('"');
                }
            }

            return null;
        }

        private static bool IsLikelyAppiumProcess(string processName, string? commandLine)
        {
            string combined = $"{processName} {commandLine}".ToLowerInvariant();
            return combined.Contains("appium", StringComparison.OrdinalIgnoreCase) ||
                   combined.Contains("appium\\build\\lib\\main.js", StringComparison.OrdinalIgnoreCase) ||
                   combined.Contains("appium/build/lib/main.js", StringComparison.OrdinalIgnoreCase);
        }

        private static string? TryGetProcessCommandLine(int pid)
        {
            try
            {
                string psCommand = $"(Get-CimInstance Win32_Process -Filter \"ProcessId={pid}\").CommandLine";
                string output = RunProcess("powershell", $"-NoProfile -Command \"{psCommand}\"");
                output = output.Trim();
                return string.IsNullOrWhiteSpace(output) ? null : output;
            }
            catch
            {
                return null;
            }
        }

        private static string RunProcess(string fileName, string arguments)
        {
            using Process process = new();
            process.StartInfo = new ProcessStartInfo(fileName, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit(5000);

            if (process.ExitCode != 0 && string.IsNullOrWhiteSpace(output))
            {
                throw new InvalidOperationException($"{fileName} {arguments} failed: {error}".Trim());
            }

            return string.IsNullOrWhiteSpace(output) ? error : output;
        }

        private sealed record PortOwnerInfo(int ProcessId, string ProcessName, string? CommandLine, bool IsLikelyAppium);

        private void TraceStartupConfiguration(TestAppConfig testAppConfig)
        {
            this.TraceAppiumSummary($"Starting Appium for {testAppConfig.Platform}.");
            this.TraceAppiumSummary($"Startup timeout: {testAppConfig.AppiumStartupTimeout.TotalSeconds} seconds.");
            this.TraceAppiumSummary($"Appium port: {AppiumPort}.");
            this.TraceAppiumSummary($"Appium log file: {this.appiumLogFilePath}.");

            if (testAppConfig.Platform == MobileTestPlatform.Android)
            {
                this.TraceAppiumSummary($"Android app path: {testAppConfig.AndroidAppPath}.");
            }
            else
            {
                this.TraceAppiumSummary($"Windows app id: {testAppConfig.WindowsAppId}.");
            }
        }

        private void TraceAppiumSummary(string? message) => this.TraceAppiumOutput(message, includeInConsole: this.appiumTraceMode != TestAppConfig.AppiumTraceMode.Off);

        private void TraceAppiumVerbose(string? message) => this.TraceAppiumOutput(message, includeInConsole: this.appiumTraceMode == TestAppConfig.AppiumTraceMode.Verbose);

        private void TraceAppiumOutput(string? message, bool includeInConsole)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            this.appiumStartupOutput.Add(message);

            if (includeInConsole)
            {
                Console.WriteLine(message);
                Debug.WriteLine(message);
            }
        }

        private string BuildStartupFailureMessage(TestAppConfig testAppConfig, Exception exception)
        {
            var message = new StringBuilder();
            message.AppendLine("Appium failed to start locally.");
            message.AppendLine($"Platform: {testAppConfig.Platform}");
            message.AppendLine($"Startup timeout: {testAppConfig.AppiumStartupTimeout.TotalSeconds} seconds");
            message.AppendLine($"Appium log file: {this.appiumLogFilePath}");

            if (testAppConfig.Platform == MobileTestPlatform.Android)
            {
                message.AppendLine($"Android app path: {testAppConfig.AndroidAppPath}");
            }
            else
            {
                message.AppendLine($"Windows app id: {testAppConfig.WindowsAppId}");
            }

            message.AppendLine($"Exception: {exception}");

            string startupLogTail = this.GetStartupLogTail();
            if (string.IsNullOrWhiteSpace(startupLogTail) == false)
            {
                message.AppendLine("Appium log tail:");
                message.AppendLine(startupLogTail);
            }

            if (this.appiumStartupOutput.Count > 0)
            {
                message.AppendLine("Captured startup output:");
                foreach (string line in this.appiumStartupOutput)
                {
                    message.AppendLine(line);
                }
            }

            return message.ToString();
        }

        public string GetStartupLogTail(int maxLines = 100)
        {
            if (string.IsNullOrWhiteSpace(this.appiumLogFilePath) || File.Exists(this.appiumLogFilePath) == false)
            {
                return string.Empty;
            }

            try
            {
                string[] lines = File.ReadLines(this.appiumLogFilePath).TakeLast(maxLines).ToArray();
                return string.Join(Environment.NewLine, lines);
            }
            catch (Exception ex)
            {
                return $"Unable to read Appium log file '{this.appiumLogFilePath}': {ex.Message}";
            }
        }

        private static void SetupWindowsDriver(Uri? appiumServerUri, TestAppConfig testAppConfig)
        {
            if (appiumServerUri is null)
            {
                throw new InvalidOperationException("Appium server URL is not available.");
            }

            var driverOptions = new AppiumOptions();
            driverOptions.AutomationName = "windows";
            driverOptions.PlatformName = "windows";
            driverOptions.DeviceName = "WindowsPC";

            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);
            driverOptions.AddAdditionalAppiumOption("ms:waitForAppLaunch", "50");
            //driverOptions.AddAdditionalAppiumOption("appium:createSessionTimeout", "100000");
            driverOptions.App = testAppConfig.WindowsAppId;
            AppiumDriverWrapper.Driver = new WindowsDriver(appiumServerUri, driverOptions, TimeSpan.FromMinutes(10));
        }

        private static void SetupAndroidDriver(Uri? appiumServerUri, TestAppConfig testAppConfig)
        {
            if (appiumServerUri is null)
            {
                throw new InvalidOperationException("Appium server URL is not available.");
            }

            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalAppiumOption("adbExecTimeout", TimeSpan.FromMinutes(5).TotalMilliseconds);
            driverOptions.AutomationName = "UIAutomator2";
            driverOptions.PlatformName = "Android";
            driverOptions.PlatformVersion = "16.0";
            driverOptions.DeviceName = "emulator-5554";

            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.transactionprocessor.mobile");
            driverOptions.AddAdditionalAppiumOption("enforceAppInstall", true);
            driverOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", "40000");
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);

            driverOptions.App = testAppConfig.AndroidAppPath;

            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.Android.AndroidDriver(appiumServerUri, driverOptions, TimeSpan.FromMinutes(5));
        }

        public List<LogEntry> GetLogs()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
            {
                if (AppiumDriverWrapper.Driver == null)
                {
                    return new List<LogEntry>();
                }

                ReadOnlyCollection<LogEntry>? logs = AppiumDriverWrapper.Driver.Manage().Logs.GetLog("logcat");
                return logs.ToList();
            }

            return new List<LogEntry>();
        }

        public async Task StopAppAsync()
        {
            try
            {
                if (AppiumDriverWrapper.Driver != null)
                {
                    try
                    {
                        AppiumDriverWrapper.Driver.Close();
                    }
                    catch
                    {
                        // Ignore close failures during teardown.
                    }

                    try
                    {
                        AppiumDriverWrapper.Driver.Quit();
                    }
                    catch
                    {
                        // Ignore quit failures during teardown.
                    }

                    try
                    {
                        AppiumDriverWrapper.Driver.Dispose();
                    }
                    catch
                    {
                        // Ignore dispose failures during teardown.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                AppiumDriverWrapper.Driver = null;

                if (this.ownsAppiumService && this.appiumService != null)
                {
                    try
                    {
                        if (this.appiumService.IsRunning)
                        {
                            this.appiumService.Dispose();
                        }
                    }
                    finally
                    {
                        this.appiumService = null;
                    }
                }

                this.appiumServerUri = null;
                this.ownsAppiumService = false;
                await Task.CompletedTask.ConfigureAwait(false);
            }
        }

        private async Task CleanupPreviousSessionAsync()
        {
            if (AppiumDriverWrapper.Driver != null || this.appiumService != null)
            {
                this.TraceAppiumSummary("Cleaning up any previous Appium session before starting a new one.");
            }

            await this.StopAppAsync().ConfigureAwait(false);
        }

        private static async Task<Uri?> TryGetRunningAppiumServerUriAsync(TimeSpan timeout)
        {
            using HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(Math.Min(Math.Max(timeout.TotalSeconds / 4, 2), 10))
            };

            Uri serverUri = new($"http://127.0.0.1:{AppiumPort}/");
            Uri statusUri = new(serverUri, "status");
            DateTime deadline = DateTime.UtcNow.Add(TimeSpan.FromSeconds(Math.Min(Math.Max(timeout.TotalSeconds / 3, 5), 15)));

            while (DateTime.UtcNow < deadline)
            {
                if (await IsAppiumStatusEndpointHealthyAsync(client, statusUri).ConfigureAwait(false))
                {
                    return serverUri;
                }

                await Task.Delay(250).ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<bool> IsAppiumStatusEndpointHealthyAsync(HttpClient client, Uri statusUri)
        {
            try
            {
                using HttpResponseMessage response = await client.GetAsync(statusUri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
