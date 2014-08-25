using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RecoveryOrderMapping : EntityTypeConfiguration<RecoveryOrder>
    {
        public RecoveryOrderMapping()
        {
            HasKey(ro => ro.Id);
            Ignore(ro => ro.Errors);
            HasRequired(ro => ro.CoreIdentification)
                .WithMany()
                .HasForeignKey(ro => ro.CoreIdentificationId)
                .WillCascadeOnDelete(false);
            HasMany(ro => ro.RecoveryOrderDetails)
                .WithRequired(rod => rod.RecoveryOrder)
                .HasForeignKey(rod => rod.RecoveryOrderId);
            HasMany(ro => ro.RollerWarehouseMutations)
                .WithRequired(rwm => rwm.RecoveryOrder)
                .HasForeignKey(rwm => rwm.RecoveryOrderId);
     
        }
    }
}