using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashBank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CurrencyId { get; set; }

        public decimal Amount { get; set; }
        public bool IsBank { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<String, String> Errors { get; set; }
        public virtual Currency Currency { get; set; }
        public ICollection<CashMutation> CashMutations { get; set; }
    }
}
