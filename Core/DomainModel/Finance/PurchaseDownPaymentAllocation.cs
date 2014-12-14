using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseDownPaymentAllocation
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int ReceivableId { get; set; }
        public string Code { get; set; }
        public DateTime AllocationDate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal RateToIDR { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Receivable Receivable { get; set; }
        public virtual ICollection<PurchaseDownPaymentAllocationDetail> PurchaseDownPaymentAllocationDetails { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
