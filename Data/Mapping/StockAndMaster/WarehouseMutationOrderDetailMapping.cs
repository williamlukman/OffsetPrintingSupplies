using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WarehouseMutationOrderDetailMapping : EntityTypeConfiguration<WarehouseMutationOrderDetail>
    {
        public WarehouseMutationOrderDetailMapping()
        {
            HasKey(wmod => wmod.Id);
            HasRequired(wmod => wmod.WarehouseMutationOrder)
                .WithMany(wmo => wmo.WarehouseMutationOrderDetails)
                .HasForeignKey(wmod => wmod.WarehouseMutationOrderId);
            HasRequired(wmod => wmod.Item)
                .WithMany()
                .HasForeignKey(wmod => wmod.ItemId)
                .WillCascadeOnDelete(false);
            Ignore(wmod => wmod.Errors);
        }
    }
}
