using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class TemporaryDeliveryOrderMapping : EntityTypeConfiguration<TemporaryDeliveryOrder>
    {
        public TemporaryDeliveryOrderMapping()
        {
            HasKey(td => td.Id);
            HasMany(td => td.TemporaryDeliveryOrderDetails)
                .WithRequired(tdod => tdod.TemporaryDeliveryOrder)
                .HasForeignKey(tdod => tdod.TemporaryDeliveryOrderId);
            HasOptional(td => td.VirtualOrder)
                .WithMany()
                .HasForeignKey(td => td.VirtualOrderId)
                .WillCascadeOnDelete(false);
            HasOptional(td => td.DeliveryOrder)
                .WithMany()
                .HasForeignKey(td => td.DeliveryOrderId)
                .WillCascadeOnDelete(false);
            HasRequired(td => td.Warehouse)
                .WithMany()
                .HasForeignKey(td => td.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(td => td.Errors);
        }
    }
}
