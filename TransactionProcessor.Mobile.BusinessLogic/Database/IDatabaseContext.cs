namespace TransactionProcessor.Mobile.BusinessLogic.Database
{
    public interface IDatabaseContext
    {
        Task InitialiseDatabase();

        Task<Int64> CreateTransaction(TransactionRecord transactionRecord);

        Task UpdateTransaction(TransactionRecord transactionRecord);

        Task<List<TransactionRecord>> GetTransactions(Boolean isTrainingMode);
        Task ClearStoredTransactions(List<TransactionRecord> transactionRecords);

        Task<List<LogMessage>> GetLogMessages(Int32 batchSize, Boolean isTrainingMode);

        Task InsertLogMessage(LogMessage logMessage);

        Task InsertLogMessages(List<LogMessage> logMessages);

        Task RemoveUploadedMessages(List<LogMessage> logMessagesToRemove);
    }
}
