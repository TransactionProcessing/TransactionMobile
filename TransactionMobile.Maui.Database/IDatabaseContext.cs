using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.Database
{
    using SQLite;

    public interface IDatabaseContext
    {
        Task InitialiseDatabase();

        Task<Int64> CreateTransaction(TransactionRecord transactionRecord);

        Task UpdateTransaction(TransactionRecord transactionRecord);

        Task<List<TransactionRecord>> GetTransactions();
    }

    public class DatabaseContext : IDatabaseContext
    {
        private readonly SQLiteConnection Connection;

        public DatabaseContext(String connectionString)
        {
            this.Connection = new SQLiteConnection(connectionString);
        }

        public async Task InitialiseDatabase()
        {
            this.Connection.CreateTable<TransactionRecord>();
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
    }

    public class TransactionRecord
    {
        public String ApplicationVersion { get; set; }
        
        public Guid ContractId { get; set; }

        public String CustomerAccountNumber { get; set; }

        public String CustomerEmailAddress { get; set; }

        public String DeviceIdentifier { get; set; }

        public String OperatorIdentifier { get; set; }

        public Guid ProductId { get; set; }

        public Decimal Amount { get; set; }

        public DateTime TransactionDateTime { get; set; }

        [PrimaryKey, AutoIncrement]
        public Int32 TransactionNumber { get; set; }

        public String RecipientMobileNumber { get; set; }
        public String RecipientEmailAddress { get; set; }

        public Int32 TransactionType { get; set; }

        public Guid EstateId { get; set; }
        public Guid MerchantId { get; set; }
        public Boolean IsSuccessful { get; set; }

        public String ResponseMessage { get; set; }

    }
}
