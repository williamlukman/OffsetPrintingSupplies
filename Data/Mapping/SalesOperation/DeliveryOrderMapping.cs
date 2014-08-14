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
            HasMany(d => d.DeliveryOrderDetails)
                .WithRequired(dod => dod.DeliveryOrder)
                .HasForeignKey(dod => dod.DeliveryOrderId);
            HasRequired(d => d.SalesOrder)
                .WithMany()
                .HasForeignKey(d => d.SalesOrderId)
                .WillCascadeOnDelete(false);
            HasRequired(d => d.Warehouse)
                .WithMany()
                .HasForeignKey(d => d.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(d => d.Errors);
        }
    }
}
