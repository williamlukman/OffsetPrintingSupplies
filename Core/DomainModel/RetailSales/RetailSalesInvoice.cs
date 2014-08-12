using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel.RetailSales
{
    public partial class RetailSalesInvoice
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime SalesDate { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public bool IsGroupPricing { get; set; }

        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmedAt { get; set; }

        public bool IsGBCH { get; set; }
        public string GBCH_No { get; set; }
        public Nullable<DateTime> GBCH_DueDate { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public bool IsFullPayment { get; set; }

        public decimal Total { get; set; }
        public decimal CoGS { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual ICollection<RetailSalesInvoiceDetail> RetailSalesinvoiceDetails { get; set; }
    }
}
