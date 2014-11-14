using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ExchangeRate
    { 
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime ExRateDate { get; set; }
        public Decimal Rate { get; set; }
        
         
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<String, String> Errors { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
