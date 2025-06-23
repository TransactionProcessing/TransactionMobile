using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;
//using Microsoft.Maui.Graphics;

[ExcludeFromCodeCoverage]
public class LogMessageModel
{
    #region Properties

    public DateTime EntryDateTime { get; set; }

    public Int32 Id { get; set; }

    public LogLevel LogLevel { get; set; }

    public String LogLevelString { get; set; }

    public String Message { get; set; }

    //public Color TextColor {
    //    get {
    //        return this.LogLevel switch {
    //            LogLevel.Debug => Colors.Gray,
    //            LogLevel.Trace => Colors.Gray,
    //            LogLevel.Info => Colors.Blue,
    //            LogLevel.Warn => Colors.Orange,
    //            LogLevel.Error => Colors.Red,
    //            LogLevel.Fatal => Colors.Red,
    //            _ => Colors.Gray
    //        };
    //    }
    //}

    #endregion
}