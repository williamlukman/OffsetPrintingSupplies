using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WarehouseMutationMapping : EntityTypeConfiguration<WarehouseMutation>
    {
        public WarehouseMutationMapping()
        {
            HasKey(wmo => wmo.Id);
            HasMany(wmo => wmo.WarehouseMutationDetails)
                .WithRequired(wmod => wmod.WarehouseMutation)
                .HasForeignKey(wmod => wmod.WarehouseMutationId);
            Ignore(wmo => wmo.Errors);
        }
    }
}
