using System.Diagnostics.CodeAnalysis;
using SQLite;

namespace TransactionProcessor.Mobile.BusinessLogic.Database
{
    [ExcludeFromCodeCoverage]
    public class ApplicationOption
    {
        [PrimaryKey]
        public String OptionName { get; set; } = String.Empty;

        public String OptionValue { get; set; } = String.Empty;
    }
}
