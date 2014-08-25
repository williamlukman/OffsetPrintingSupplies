using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RollerWarehouseMutationDetailMapping : EntityTypeConfiguration<RollerWarehouseMutationDetail>
    {
        public RollerWarehouseMutationDetailMapping()
        {
            HasKey(rwmd => rwmd.Id);
            HasRequired(rwmd => rwmd.RollerWarehouseMutation)
                .WithMany(rwm => rwm.RollerWarehouseMutationDetails)
                .HasForeignKey(rwmd => rwmd.RollerWarehouseMutationId);
            // supposed to be 1 to 0-1 relationship rather than 1 to many.
            // changing WithMany to WithOptional causes error in the EF
            HasRequired(rwmd => rwmd.RecoveryOrderDetail)
                .WithMany()
                .HasForeignKey(rwmd => rwmd.RecoveryOrderDetailId);
            Ignore(rwmd => rwmd.Errors);
        }
    }
}
