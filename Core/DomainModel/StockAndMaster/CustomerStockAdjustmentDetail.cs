using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CustomerStockAdjustmentDetail
    {
        public int Id { get; set; }
        public int CustomerStockAdjustmentId { get; set; }
        //public int ContactId { get; set; }
        public int ItemId { get; set; }
        public string Code { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        
        public virtual Item Item { get; set; }
        //public virtual Contact Contact { get; set; }
        public virtual CustomerStockAdjustment CustomerStockAdjustment { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}