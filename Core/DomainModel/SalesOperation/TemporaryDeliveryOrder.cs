using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TemporaryDeliveryOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int OrderType { get; set; }
        public Nullable<int> VirtualOrderId { get; set; }
        public Nullable<int> DeliveryOrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int WarehouseId { get; set; }
        public string NomorSurat { get; set; }

        public decimal TotalWasteCOGS { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeliveryCompleted { get; set; } // DeliveryOrder has been confirmed, Restock has been processed
        public bool IsReconciled { get; set; } // Waste + Restock = Quantity, Waste has been thrown
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<TemporaryDeliveryOrderDetail> TemporaryDeliveryOrderDetails { get; set; }
        public virtual ICollection<TemporaryDeliveryOrderClearance> TemporaryDeliveryOrderClearances { get; set; }
        public virtual VirtualOrder VirtualOrder { get; set; }
        public virtual DeliveryOrder DeliveryOrder { get; set; }
        public Dictionary<String, String> Errors { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}