using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class UoMMapping : EntityTypeConfiguration<UoM>
    {
        public UoMMapping()
        {
            HasKey(uom => uom.Id);
            HasMany(uom => uom.Items)
                .WithRequired(i => i.UoM)
                .HasForeignKey(i => i.UoMId);
            HasMany(uom => uom.CoreBuilders)
                .WithRequired(cb => cb.UoM)
                .HasForeignKey(cb => cb.UoMId);
            HasMany(uom => uom.RollerBuilders)
                .WithRequired(rb => rb.UoM)
                .HasForeignKey(rb => rb.UoMId);
            Ignore(uom => uom.Errors);
        }
    }
}