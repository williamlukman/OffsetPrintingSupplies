using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RollerBuilderMapping : EntityTypeConfiguration<RollerBuilder>
    {
        public RollerBuilderMapping()
        {
            HasKey(rb => rb.Id);
            HasRequired(rb => rb.CoreBuilder)
                .WithMany(cb => cb.RollerBuilders)
                .HasForeignKey(rb => rb.CoreBuilderId);
            HasRequired(rb => rb.Machine)
                .WithMany(m => m.RollerBuilders)
                .HasForeignKey(rb => rb.MachineId);
            HasRequired(rb => rb.RollerType)
                .WithMany(rt => rt.RollerBuilders)
                .HasForeignKey(rb => rb.RollerTypeId);
            Ignore(rb => rb.Errors);
        }
    }
}