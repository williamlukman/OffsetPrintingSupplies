using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    //For CashBank,AR & AP non BaseCurrency
    public partial class GLNonBaseCurrency
    { 
        public int Id { get; set; }
        public int GeneralLedgerJournalId { get; set; }
         
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; } 

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        
        public virtual GeneralLedgerJournal GeneralLedgerJournal { get; set; }
        public virtual Currency Currency { get; set; }

    }
}
