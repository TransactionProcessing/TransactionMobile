namespace TransactionMobile.Maui.BusinessLogic.Models;

public class AppCenterConfiguration
{
    public String AndroidKey { get; set; }
    public String iOSKey { get; set; }
    public String MacOSKey { get; set; }
    public String WindowsKey { get; set; }

    public String GetAppCenterKey() {
        List<String> apiKeys = new List<String>();
        if (String.IsNullOrEmpty(AndroidKey) == false) {
            apiKeys.Add($"android={this.AndroidKey};");
        }
        if (String.IsNullOrEmpty(iOSKey) == false)
        {
            apiKeys.Add($"ios={this.iOSKey};");
        }
        if (String.IsNullOrEmpty(WindowsKey) == false)
        {
            apiKeys.Add($"uwp={this.WindowsKey};");
        }
        if (String.IsNullOrEmpty(MacOSKey) == false)
        {
            apiKeys.Add($"macos={this.MacOSKey};");
        }

        return String.Join("+", apiKeys);
    }
}