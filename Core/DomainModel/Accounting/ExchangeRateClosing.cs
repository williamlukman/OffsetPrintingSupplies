using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{ 
    public partial class ExchangeRateClosing
    {
        public int Id { get; set; }
        public int ClosingId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Rate { get; set; } 

        public Dictionary<string, string> Errors { get; set; }
        public virtual Closing Closing { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
