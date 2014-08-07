using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string PIC { get; set; }
        public string PICContactNo { get; set; }
        public string Email { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public ICollection<Barring> Barrings { get; set; }
        public ICollection<CoreIdentification> CoreIdentifications { get; set; }
        public ICollection<BarringOrder> BarringOrders { get; set; }
        public ICollection<SalesOrder> SalesOrders { get; set; }
        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
        public ICollection<DeliveryOrder> DeliveryOrders { get; set; }
        public ICollection<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public ICollection<PurchaseReceival> PurchaseReceivals { get; set; }
        public ICollection<PurchaseReceivalDetail> PurchaseReceivalDetails { get; set; }
    }
}
