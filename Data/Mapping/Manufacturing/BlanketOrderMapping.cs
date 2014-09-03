using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlanketOrderMapping : EntityTypeConfiguration<BlanketOrder>
    {
        public BlanketOrderMapping()
        {
            HasKey(bo => bo.Id);
            HasMany(bo => bo.BlanketOrderDetails)
                .WithRequired(bod => bod.BlanketOrder)
                .HasForeignKey(bod => bod.BlanketOrderId);
            HasRequired(bo => bo.Contact)
                .WithMany(c => c.BlanketOrders)
                .HasForeignKey(bo => bo.ContactId);
            HasRequired(bo => bo.Warehouse)
                .WithMany()
                .HasForeignKey(bo => bo.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(bo => bo.Errors);
        }
    }
}