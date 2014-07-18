using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Ignore(uom => uom.Errors);
        }
    }
}