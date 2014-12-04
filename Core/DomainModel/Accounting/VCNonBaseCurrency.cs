using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    //For CashBank,AR & AP non BaseCurrency
    public partial class VCNonBaseCurrency
    {
        public int Id { get; set; }
        public int ValidCombId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
         
        public virtual ValidComb ValidComb { get; set; }

    }
}
