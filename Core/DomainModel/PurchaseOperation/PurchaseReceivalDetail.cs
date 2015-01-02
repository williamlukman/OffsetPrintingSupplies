using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseReceivalDetail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int PurchaseReceivalId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public bool IsAllInvoiced { get; set; }
        public decimal PendingInvoicedQuantity { get; set; }
        public int PurchaseOrderDetailId { get; set; }

        public decimal COGS { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual Item Item { get; set; }
        public virtual PurchaseReceival PurchaseReceival { get; set; }
        public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}