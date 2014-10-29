using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesQuotation
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string VersionNo { get; set; }
        public string NomorSurat { get; set; }

        public int ContactId { get; set; }
        public DateTime QuotationDate { get; set; }

        public decimal TotalQuotedAmount { get; set; }
        public decimal TotalRRPAmount { get; set; }
        public decimal CostSaved { get; set; }
        public decimal PercentageSaved { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsSalesOrderConfirmed { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<SalesQuotationDetail> SalesQuotationDetails { get; set; }
        public virtual Contact Contact {get; set;}
        public Dictionary<String, String> Errors { get; set; }
    }
}