using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CustomerItem // CustomerWarehouseItem ?
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        //public int ItemId { get; set; }
        public Nullable<int> WarehouseItemId { get; set; }

        public int Quantity { get; set; } // Qty per contact per item per warehouse
        public int Virtual { get; set; } // unused ? (virtualorder can only sell company's item)

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual Contact Contact { get; set; }
        //public virtual Item Item { get; set; }
        public virtual WarehouseItem WarehouseItem { get; set; }
    }
}
