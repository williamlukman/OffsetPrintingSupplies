using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BarringOrderMapping : EntityTypeConfiguration<BarringOrder>
    {
        public BarringOrderMapping()
        {
            HasKey(bo => bo.Id);
            HasMany(bo => bo.BarringOrderDetails)
                .WithRequired(bod => bod.BarringOrder)
                .HasForeignKey(bod => bod.BarringOrderId);
            HasRequired(bo => bo.Contact)
                .WithMany(c => c.BarringOrders)
                .HasForeignKey(bo => bo.ContactId);
            HasRequired(bo => bo.Warehouse)
                .WithMany()
                .HasForeignKey(bo => bo.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(bo => bo.Errors);
        }
    }
}