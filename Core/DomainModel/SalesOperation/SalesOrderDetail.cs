using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesOrderDetail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string OrderCode { get; set; }
        public int SalesOrderId { get; set; }
        public int ItemId { get; set; }

        public decimal Quantity { get; set; }
        public bool IsAllDelivered { get; set; }
        public decimal PendingDeliveryQuantity { get; set; }
        public decimal Price { get; set; }

        public bool IsService { get; set; } // IsRepair

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual Item Item { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}