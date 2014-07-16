using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class WarehouseMutationOrderMapping : EntityTypeConfiguration<WarehouseMutationOrder>
    {
        public WarehouseMutationOrderMapping()
        {
            HasKey(wmo => wmo.Id);
            HasMany(wmo => wmo.WarehouseMutationOrderDetails)
                .WithRequired(wmod => wmod.WarehouseMutationOrder)
                .HasForeignKey(wmod => wmod.WarehouseMutationOrderId);
            Ignore(wmo => wmo.Errors);
        }
    }
}
