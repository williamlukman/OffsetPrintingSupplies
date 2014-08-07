using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class DeliveryOrderMapping : EntityTypeConfiguration<DeliveryOrder>
    {
        public DeliveryOrderMapping()
        {
            HasKey(d => d.Id);
            HasRequired(d => d.Contact)
                .WithMany(c => c.DeliveryOrders)
                .HasForeignKey(d => d.ContactId);
            HasMany(d => d.DeliveryOrderDetails)
                .WithRequired(dod => dod.DeliveryOrder)
                .HasForeignKey(dod => dod.DeliveryOrderId);
            Ignore(d => d.Errors);
        }
    }
}
