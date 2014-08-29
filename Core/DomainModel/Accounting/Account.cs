using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Account
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public int Group { get; set; }
        public int Level { get; set; }
        public int ParentId { get; set; }
        public bool IsLegacy { get; set; }
        public bool IsCashBankAccount { get; set; }

        public int LegacyCode { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual ICollection<GeneralLedgerJournal> GeneralLedgerJournals { get; set; }
    }
}
