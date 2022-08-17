namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Support;

using Models;

public class LogMessageViewModel
{
    #region Properties

    public DateTime EntryDateTime { get; set; }

    public Int32 Id { get; set; }

    public LogLevel LogLevel { get; set; }

    public String LogLevelString { get; set; }

    public String Message { get; set; }

    public Color TextColor {
        get {
            return this.LogLevel switch {
                LogLevel.Debug => Colors.Gray,
                LogLevel.Trace => Colors.Gray,
                LogLevel.Info => Colors.Blue,
                LogLevel.Warn => Colors.Orange,
                LogLevel.Error => Colors.Red,
                LogLevel.Fatal => Colors.Red,
                _ => Colors.Gray
            };
        }
    }

    #endregion
}