using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlanketWarehouseMutationMapping : EntityTypeConfiguration<BlanketWarehouseMutation>
    {
        public BlanketWarehouseMutationMapping()
        {
            HasKey(rwm => rwm.Id);
            HasMany(rwm => rwm.BlanketWarehouseMutationDetails)
                .WithRequired(rwmd => rwmd.BlanketWarehouseMutation)
                .HasForeignKey(rwmd => rwmd.BlanketWarehouseMutationId);
            HasRequired(rwm => rwm.BlanketOrder)
                .WithMany(ro => ro.BlanketWarehouseMutations)
                .HasForeignKey(rwm => rwm.BlanketOrderId);
            HasRequired(rwm => rwm.WarehouseFrom)
                .WithMany()
                .HasForeignKey(rwm => rwm.WarehouseFromId)
                .WillCascadeOnDelete(false);
            HasRequired(rwm => rwm.WarehouseTo)
                .WithMany()
                .HasForeignKey(rwm => rwm.WarehouseToId)
                .WillCascadeOnDelete(false);
            Ignore(rwm => rwm.Errors);
        }
    }
}
