using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CustomerStockAdjustment
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ContactId { get; set; }

        public DateTime AdjustmentDate { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<CustomerStockAdjustmentDetail> CustomerStockAdjustmentDetails { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}