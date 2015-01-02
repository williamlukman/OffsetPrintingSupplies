using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class WarehouseItem
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ItemId { get; set; }
        //public int CustomerItemId { get; set; }

        public decimal Quantity { get; set; } // Company's Item Quantity (excluding customer's)
        public decimal PendingDelivery { get; set; }
        public decimal PendingReceival { get; set; }
        public decimal CustomerQuantity { get; set; } // Customer's Item Quantity

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<CustomerItem> CustomerItems { get; set; }
    }
}
