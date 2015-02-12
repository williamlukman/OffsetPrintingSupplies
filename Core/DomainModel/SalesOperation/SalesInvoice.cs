using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesInvoice
    {
        public int Id { get; set; }
        public int DeliveryOrderId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string NomorSurat { get; set; }
        //public string NomorFakturPajak { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRateAmount { get; set; }

        public decimal TotalCOS { get; set; }
        public decimal AmountReceivable { get; set; }
        public decimal Discount { get; set; } // 0 - 100
        public decimal DPP { get; set; }
        public decimal Tax { get; set; } // 0 - 100
        public Nullable<int> ExchangeRateId { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual DeliveryOrder DeliveryOrder { get; set; }
        public virtual ICollection<SalesInvoiceDetail> SalesInvoiceDetails { get; set; }
        public Dictionary<String, String> Errors { get; set; }
        public virtual Currency Currency { get; set; } 
        public virtual ExchangeRate ExchangeRate { get; set; }
    }
}
