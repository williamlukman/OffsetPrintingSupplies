using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TemporaryDeliveryOrderClearance
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Nullable<int> TemporaryDeliveryOrderId { get; set; }
        public DateTime ClearanceDate { get; set; }
        //public int WarehouseId { get; set; }
        //public string NomorSurat { get; set; }

        public decimal TotalWasteCOGS { get; set; }

        public bool IsWasted { get; set; } // ClearanceType
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        //public bool IsDeliveryCompleted { get; set; } // DeliveryOrder has been confirmed, Restock has been processed
        //public bool IsReconciled { get; set; } // Waste + Restock = Quantity, Waste has been thrown
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<TemporaryDeliveryOrderClearanceDetail> TemporaryDeliveryOrderClearanceDetails { get; set; }
        //public virtual VirtualOrder VirtualOrder { get; set; }
        public virtual TemporaryDeliveryOrder TemporaryDeliveryOrder { get; set; }
        public Dictionary<String, String> Errors { get; set; }
        //public virtual Warehouse Warehouse { get; set; }
    }
}