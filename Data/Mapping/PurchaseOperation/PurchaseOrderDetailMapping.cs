using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseOrderDetailMapping : EntityTypeConfiguration<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailMapping()
        {
            HasKey(pod => pod.Id);
            HasRequired(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            HasRequired(pod => pod.Item)
                .WithMany()
                .HasForeignKey(pod => pod.ItemId)
                .WillCascadeOnDelete(false);
            Ignore(pod => pod.Errors);
        }
    }
}
