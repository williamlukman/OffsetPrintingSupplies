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
            HasRequired(ex => ex.Currency)
                 .WithMany()
                 .HasForeignKey(ex => ex.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasMany(po => po.PurchaseOrderDetails)
                .WithRequired(pod => pod.PurchaseOrder)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            Ignore(po => po.Errors);
        }
    }
}
