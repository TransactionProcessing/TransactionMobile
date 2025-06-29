﻿using System.Diagnostics.CodeAnalysis;
using SQLite;

namespace TransactionProcessor.Mobile.BusinessLogic.Database
{
    [ExcludeFromCodeCoverage]
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
}
