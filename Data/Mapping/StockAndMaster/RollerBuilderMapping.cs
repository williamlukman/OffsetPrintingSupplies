using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RollerBuilderMapping : EntityTypeConfiguration<RollerBuilder>
    {
        public RollerBuilderMapping()
        {
            HasKey(rb => rb.Id);
            HasRequired(rb => rb.CoreBuilder)
                .WithMany(cb => cb.RollerBuilders)
                .HasForeignKey(rb => rb.CoreBuilderId)
                .WillCascadeOnDelete(false);
            HasRequired(rb => rb.Machine)
                .WithMany(m => m.RollerBuilders)
                .HasForeignKey(rb => rb.MachineId);
            HasRequired(rb => rb.RollerType)
                .WithMany(rt => rt.RollerBuilders)
                .HasForeignKey(rb => rb.RollerTypeId);
            HasRequired(rb => rb.UoM)
                .WithMany(uom => uom.RollerBuilders)
                .HasForeignKey(rb => rb.UoMId);
            HasRequired(rb => rb.Adhesive)
                .WithMany()
                .HasForeignKey(rb => rb.AdhesiveId)
                .WillCascadeOnDelete(false);
            HasRequired(rb => rb.RollerUsedCoreItem)
                .WithMany()
                .HasForeignKey(rb => rb.RollerUsedCoreItemId)
                .WillCascadeOnDelete(false);
            Ignore(rb => rb.Errors);
        }
    }
}