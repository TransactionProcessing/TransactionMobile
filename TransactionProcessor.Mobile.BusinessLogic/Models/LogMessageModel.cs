using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessor.Mobile.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class LogMessageModel
{
    public DateTime EntryDateTime { get; set; }

    public Int32 Id { get; set; }

    public LogLevel LogLevel { get; set; }

    public String LogLevelString { get; set; }

    public String Message { get; set; }
}