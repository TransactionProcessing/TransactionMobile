namespace TransactionMobile.Maui.Database
{
    using SQLite;
    public class LogMessage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the entry date time.
        /// </summary>
        /// <value>
        /// The entry date time.
        /// </value>
        public DateTime EntryDateTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [PrimaryKey, AutoIncrement]
        public Int32 Id { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        public String LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public String Message { get; set; }

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
