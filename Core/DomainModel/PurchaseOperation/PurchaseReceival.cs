using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseReceival
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int PurchaseOrderId { get; set; }
        public DateTime ReceivalDate { get; set; }
        public int WarehouseId { get; set; }
        public string NomorSurat { get; set; }

        public decimal TotalCOGS { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsInvoiceCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual ICollection<PurchaseReceivalDetail> PurchaseReceivalDetails { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}