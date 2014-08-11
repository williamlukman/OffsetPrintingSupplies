using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RecoveryOrderDetailMapping : EntityTypeConfiguration<RecoveryOrderDetail>
    {
        public RecoveryOrderDetailMapping()
        {
            HasKey(rod => rod.Id);
            HasRequired(rod => rod.CoreIdentificationDetail)
                .WithMany()
                .HasForeignKey(rod => rod.CoreIdentificationDetailId)
                .WillCascadeOnDelete(false);
            HasRequired(rod => rod.RecoveryOrder)
                .WithMany(ro => ro.RecoveryOrderDetails)
                .HasForeignKey(rod => rod.RecoveryOrderId);
            HasRequired(rod => rod.RollerBuilder)
                .WithMany()
                .HasForeignKey(rod => rod.RollerBuilderId)
                .WillCascadeOnDelete(false);
            HasMany(rod => rod.RecoveryAccessoryDetails)
                .WithRequired(rad => rad.RecoveryOrderDetail)
                .HasForeignKey(rad => rad.RecoveryOrderDetailId);
            Ignore(rod => rod.Errors);
        }
    }
}