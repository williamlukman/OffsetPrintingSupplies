using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RollerWarehouseMutationMapping : EntityTypeConfiguration<RollerWarehouseMutation>
    {
        public RollerWarehouseMutationMapping()
        {
            HasKey(rwm => rwm.Id);
            HasMany(rwm => rwm.RollerWarehouseMutationDetails)
                .WithRequired(rwmd => rwmd.RollerWarehouseMutation)
                .HasForeignKey(rwmd => rwmd.RollerWarehouseMutationId);
            HasRequired(rwm => rwm.RecoveryOrder)
                .WithMany(ro => ro.RollerWarehouseMutations)
                .HasForeignKey(rwm => rwm.RecoveryOrderId);
            Ignore(rwm => rwm.Errors);
        }
    }
}
