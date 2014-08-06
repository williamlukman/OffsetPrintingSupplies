using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RollerTypeMapping : EntityTypeConfiguration<RollerType>
    {
        public RollerTypeMapping()
        {
            HasKey(rt => rt.Id);
            HasMany(rt => rt.RollerBuilders)
                .WithRequired(rb => rb.RollerType)
                .HasForeignKey(rb => rb.RollerTypeId);
            Ignore(rt => rt.Errors);
        }
    }
}