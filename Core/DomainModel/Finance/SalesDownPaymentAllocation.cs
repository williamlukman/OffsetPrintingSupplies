using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesDownPaymentAllocation
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int SalesDownPaymentId { get; set; }
        public int PayableId { get; set; }
        public string Code { get; set; }
        public DateTime AllocationDate { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual SalesDownPayment SalesDownPayment { get; set; }
        public virtual Payable Payable { get; set; }
        public virtual ICollection<SalesDownPaymentAllocationDetail> SalesDownPaymentAllocationDetails { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
