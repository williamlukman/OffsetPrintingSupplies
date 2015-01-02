using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesQuotationDetail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SalesQuotationId { get; set; }
        public int ItemId { get; set; }

        public decimal Quantity { get; set; }
        public decimal RRP { get; set; }
        public decimal QuotationPrice { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual Item Item { get; set; }
        public virtual SalesQuotation SalesQuotation { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}