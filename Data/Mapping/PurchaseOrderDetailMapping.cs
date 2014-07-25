using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class PurchaseOrderDetailMapping : EntityTypeConfiguration<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailMapping()
        {
            HasKey(pod => pod.Id);
            HasRequired(pod => pod.Customer)
                .WithMany(c => c.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.CustomerId);
            HasRequired(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            Ignore(pod => pod.Errors);
        }
    }
}
