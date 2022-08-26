namespace TransactionMobile.Maui.Database
{
    using SQLite;
    public class LogMessage
    {
        #region Properties
        
        public DateTime EntryDateTime { get; set; }
        
        [PrimaryKey, AutoIncrement]
        public Int32 Id { get; set; }
        
        public String LogLevel { get; set; }
        
        public String Message { get; set; }

        public Boolean IsTrainingMode { get; set; }

        #endregion
    }

    public enum LogLevel
    {
        /// <summary>
        /// The fatal
        /// </summary>
        Fatal,

        /// <summary>
        /// The error
        /// </summary>
        Error,

        /// <summary>
        /// The warn
        /// </summary>
        Warn,

        /// <summary>
        /// The information
        /// </summary>
        Info,

        /// <summary>
        /// The debug
        /// </summary>
        Debug,

        /// <summary>
        /// The trace
        /// </summary>
        Trace
    }
}
