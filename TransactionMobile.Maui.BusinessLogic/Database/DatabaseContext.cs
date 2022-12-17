namespace TransactionMobile.Maui.Database
{
    using SQLite;

    public class DatabaseContext : IDatabaseContext
    {
        #region Fields

        private readonly SQLiteConnection Connection;

        private readonly Func<LogLevel> LogLevelResolver;

        #endregion

        #region Constructors

        public DatabaseContext(String connectionString,
                               Func<LogLevel> logLevelResolver) {
            this.Connection = new SQLiteConnection(connectionString);
            this.LogLevelResolver = logLevelResolver;
        }

        #endregion

        #region Methods

        public async Task ClearStoredTransactions(List<TransactionRecord> transactionRecords) {
            foreach (var transactionRecord in transactionRecords) {
                this.Connection.Delete<TransactionRecord>(transactionRecord.TransactionNumber);
            }
        }

        public async Task<Int64> CreateTransaction(TransactionRecord transactionRecord) {
            this.Connection.Insert(transactionRecord);

            return SQLite3.LastInsertRowid(this.Connection.Handle);
        }

        public async Task<List<LogMessage>> GetLogMessages(Int32 batchSize, Boolean isTrainingMode) {
            if (this.Connection == null)
                return new List<LogMessage>();

            List<LogMessage> messages = this.Connection.Table<LogMessage>()
                                            .Where(l => l.IsTrainingMode == isTrainingMode)
                                            .OrderByDescending(l => l.EntryDateTime)
                                            .Take(batchSize).ToList();

            return messages;
        }

        public async Task<List<TransactionRecord>> GetTransactions(Boolean isTrainingMode) {
            return this.Connection.Table<TransactionRecord>().Where(t => t.IsTrainingMode == isTrainingMode).ToList();
        }
        
        public async Task InitialiseDatabase() {
            this.Connection.CreateTable<TransactionRecord>();
            this.Connection.CreateTable<LogMessage>();
        }

        public async Task InsertLogMessage(LogMessage logMessage) {
            if (this.Connection == null)
                return;

            LogLevel messageLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logMessage.LogLevel, true);
            if (messageLevel <= this.LogLevelResolver()) {
                this.Connection.Insert(logMessage);
            }
        }

        public async Task InsertLogMessages(List<LogMessage> logMessages) {
            foreach (LogMessage logMessage in logMessages) {
                await this.InsertLogMessage(logMessage);
            }
        }

        public async Task RemoveUploadedMessages(List<LogMessage> logMessagesToRemove) {
            if (this.Connection == null)
                return;
            foreach (LogMessage logMessage in logMessagesToRemove) {
                this.Connection.Delete(logMessage);
            }
        }

        public async Task UpdateTransaction(TransactionRecord transactionRecord) {
            this.Connection.Update(transactionRecord);
        }

        #endregion
    }
}