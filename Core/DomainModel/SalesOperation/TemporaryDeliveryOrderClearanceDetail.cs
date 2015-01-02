using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TemporaryDeliveryOrderClearanceDetail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int TemporaryDeliveryOrderClearanceId { get; set; }
        //public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        //public bool IsReconciled { get; set; }
        //public bool IsAllCompleted { get; set; }
        public decimal WasteCoGS { get; set; }
        public decimal SellingPrice { get; set; }
        //public decimal WasteQuantity { get; set; }
        //public decimal ReturnQuantity { get; set; }
        //public Nullable<int> SalesOrderDetailId { get; set; }
        //public Nullable<int> VirtualOrderDetailId { get; set; }
        public Nullable<int> TemporaryDeliveryOrderDetailId { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        //public virtual Item Item { get; set; }
        public virtual TemporaryDeliveryOrderClearance TemporaryDeliveryOrderClearance { get; set; }
        //public virtual SalesOrderDetail SalesOrderDetail { get; set; }
        //public virtual VirtualOrderDetail VirtualOrderDetail { get; set; }
        public virtual TemporaryDeliveryOrderDetail TemporaryDeliveryOrderDetail { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}