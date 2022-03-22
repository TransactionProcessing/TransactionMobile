﻿namespace TransactionMobile.Maui.Database
{
    using SQLite;

    public class DatabaseContext : IDatabaseContext
    {
        private readonly SQLiteConnection Connection;

        private readonly Func<LogLevel> LogLevelResolver;

        public DatabaseContext(String connectionString, Func<LogLevel> logLevelResolver)
        {
            this.Connection = new SQLiteConnection(connectionString);
            this.LogLevelResolver = logLevelResolver;
        }

        public async Task InitialiseDatabase()
        {
            this.Connection.CreateTable<TransactionRecord>();
            this.Connection.CreateTable<LogMessage>();
        }

        public async Task<Int64> CreateTransaction(TransactionRecord transactionRecord)
        {
            this.Connection.Insert(transactionRecord);

            return SQLite3.LastInsertRowid(this.Connection.Handle);
        }

        public async Task UpdateTransaction(TransactionRecord transactionRecord)
        {
            this.Connection.Update(transactionRecord);
        }

        public async Task<List<TransactionRecord>> GetTransactions()
        {
            return this.Connection.Table<TransactionRecord>().ToList();
        }

        public async Task ClearStoredTransactions()
        {
            this.Connection.DeleteAll<TransactionRecord>();
        }

        public async Task<List<LogMessage>> GetLogMessages(int batchSize)
        {
            if (this.Connection == null)
                return new List<LogMessage>();

            List<LogMessage> messages = this.Connection.Table<LogMessage>().OrderBy(l => l.EntryDateTime).Take(batchSize).ToList();

            return messages;
        }

        public async Task InsertLogMessage(LogMessage logMessage)
        {
            if (this.Connection == null)
                return;

            LogLevel messageLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logMessage.LogLevel, true);
            if (messageLevel <= this.LogLevelResolver())
            {
            this.Connection.Insert(logMessage);
            }
        }

        public async Task InsertLogMessages(List<LogMessage> logMessages)
        {
            foreach (LogMessage logMessage in logMessages)
            {
                await this.InsertLogMessage(logMessage);
            }
        }

        public async Task RemoveUploadedMessages(List<LogMessage> logMessagesToRemove)
        {
            if (this.Connection == null)
                return;
            foreach (LogMessage logMessage in logMessagesToRemove)
            {
                this.Connection.Delete(logMessage);
            }
        }
    }
}
