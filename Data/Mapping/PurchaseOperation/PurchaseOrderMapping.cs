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
            HasRequired(po => po.Contact)
                .WithMany(c => c.PurchaseOrders)
                .HasForeignKey(po => po.ContactId);
            HasRequired(po => po.Currency)
                 .WithMany()
                 .HasForeignKey(po => po.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasMany(po => po.PurchaseOrderDetails)
                .WithRequired(pod => pod.PurchaseOrder)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            Ignore(po => po.Errors);
        }
    }
}
