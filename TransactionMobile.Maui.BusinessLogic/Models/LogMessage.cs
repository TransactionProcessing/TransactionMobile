using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Models
{
    public class LogMessage
    {
        #region Properties

        public DateTime EntryDateTime { get; set; }

        public Int32 Id { get; set; }

        public String LogLevel { get; set; }

        public String Message { get; set; }

        #endregion
    }
}
