using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.Database
{
    public interface IDatabaseContext
    {
        Task InitialiseDatabase();

        Task<Int64> CreateTransaction(TransactionRecord transactionRecord);

        Task UpdateTransaction(TransactionRecord transactionRecord);

        Task<List<TransactionRecord>> GetTransactions();
        Task ClearStoredTransactions(List<TransactionRecord> transactionRecords);

        Task<List<LogMessage>> GetLogMessages(Int32 batchSize);

        Task InsertLogMessage(LogMessage logMessage);

        Task InsertLogMessages(List<LogMessage> logMessages);

        Task RemoveUploadedMessages(List<LogMessage> logMessagesToRemove);
    }
}
