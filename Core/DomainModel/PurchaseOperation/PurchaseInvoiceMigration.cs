using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseInvoiceMigration
    {
        public int Id { get; set; }
        public string NomorSurat { get; set; }
        public int ContactId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Rate { get; set; }

        public decimal AmountPayable { get; set; }
        public decimal DPP { get; set; }
        public decimal Tax { get; set; }
        
        public DateTime InvoiceDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<String, String> Errors { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Currency Currency { get; set; }

    }
}
