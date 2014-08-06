using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseOrderMapping : EntityTypeConfiguration<PurchaseOrder>
    {
        public PurchaseOrderMapping()
        {
            HasKey(po => po.Id);
            HasRequired(po => po.Customer)
                .WithMany(c => c.PurchaseOrders)
                .HasForeignKey(po => po.CustomerId);
            HasMany(po => po.PurchaseOrderDetails)
                .WithRequired(pod => pod.PurchaseOrder)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            Ignore(po => po.Errors);
        }
    }
}
