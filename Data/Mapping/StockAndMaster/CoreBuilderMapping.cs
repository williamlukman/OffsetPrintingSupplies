using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CoreBuilderMapping : EntityTypeConfiguration<CoreBuilder>
    {
        public CoreBuilderMapping()
        {
            HasKey(cb => cb.Id);
            HasMany(cb => cb.CoreIdentificationDetails)
                .WithRequired(cid => cid.CoreBuilder)
                .HasForeignKey(cid => cid.CoreBuilderId);
            HasMany(cb => cb.RollerBuilders)
                .WithRequired(rb => rb.CoreBuilder)
                .HasForeignKey(rb => rb.CoreBuilderId);
            HasRequired(cb => cb.UoM)
                .WithMany(uom => uom.CoreBuilders)
                .HasForeignKey(cb => cb.UoMId);
            HasRequired(cb => cb.Machine)
                .WithMany()
                .HasForeignKey(cb => cb.MachineId)
                .WillCascadeOnDelete(false);
            Ignore(cb => cb.Errors);
        }
    }
}