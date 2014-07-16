using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class WarehouseItemMapping : EntityTypeConfiguration<WarehouseItem>
    {
        public WarehouseItemMapping()
        {
            HasKey(wi => wi.Id);
            HasRequired(wi => wi.Item)
                .WithMany(i => i.WarehouseItems)
                .HasForeignKey(wi => wi.ItemId);
            HasRequired(wi => wi.Warehouse)
                .WithMany(w => w.WarehouseItems)
                .HasForeignKey(wi => wi.WarehouseId);
            Ignore(wi => wi.Errors);
        }
    }
}
