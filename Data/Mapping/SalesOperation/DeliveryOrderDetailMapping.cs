using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class DeliveryOrderDetailMapping : EntityTypeConfiguration<DeliveryOrderDetail>
    {
        public DeliveryOrderDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(dod => dod.Contact)
                .WithMany(c => c.DeliveryOrderDetails)
                .HasForeignKey(dod => dod.ContactId);
            HasRequired(dod => dod.DeliveryOrder)
                .WithMany(d => d.DeliveryOrderDetails)
                .HasForeignKey(dod => dod.DeliveryOrderId);
            HasRequired(dod => dod.SalesOrderDetail)
                .WithMany()
                .HasForeignKey(dod => dod.SalesOrderDetailId);
            Ignore(pod => pod.Errors);
        }
    }
}
