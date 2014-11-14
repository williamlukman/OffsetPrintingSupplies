using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseInvoice
    {
        public int Id { get; set; }
        public int PurchaseReceivalId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string NomorSurat { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRateAmount { get; set; }

        public decimal AmountPayable { get; set; }
        public decimal Discount { get; set; } // 0 - 100 %
        public bool IsTaxable { get; set; } // 10 %
        public decimal Tax { get; set; }
        
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual PurchaseReceival PurchaseReceival { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public virtual Currency Currency { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
