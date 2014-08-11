using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WarehouseMapping : EntityTypeConfiguration<Warehouse>
    {
        public WarehouseMapping()
        {
            HasKey(w => w.Id);
            HasMany(w => w.WarehouseItems)
                .WithRequired(wi => wi.Warehouse)
                .HasForeignKey(wi => wi.WarehouseId);
            Ignore(w => w.Errors);
        }
    }
}
