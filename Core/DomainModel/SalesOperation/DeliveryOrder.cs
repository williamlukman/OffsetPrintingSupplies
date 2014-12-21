using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class DeliveryOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int WarehouseId { get; set; }
        public string NomorSurat { get; set; }
        public decimal ExchangeRateAmount { get; set; }
        public Nullable<int> ExchangeRateId { get; set; }

        public decimal TotalCOGS { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsInvoiceCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public virtual SalesOrder SalesOrder {get; set;}
        public Dictionary<String, String> Errors { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}