using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WarehouseMutationOrderMapping : EntityTypeConfiguration<WarehouseMutationOrder>
    {
        public WarehouseMutationOrderMapping()
        {
            HasKey(wmo => wmo.Id);
            HasMany(wmo => wmo.WarehouseMutationOrderDetails)
                .WithRequired(wmod => wmod.WarehouseMutationOrder)
                .HasForeignKey(wmod => wmod.WarehouseMutationOrderId);
            HasRequired(wmo => wmo.WarehouseFrom)
                .WithMany()
                .HasForeignKey(wmo => wmo.WarehouseFromId)
                .WillCascadeOnDelete(false);
            HasRequired(wmo => wmo.WarehouseTo)
                .WithMany()
                .HasForeignKey(wmo => wmo.WarehouseToId)
                .WillCascadeOnDelete(false);
            Ignore(wmo => wmo.Errors);
        }
    }
}
