using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel.RetailSales;

namespace Data.Mapping.RetailSales
{
    public class PriceMutationMapping : EntityTypeConfiguration<PriceMutation>
    {
        public PriceMutationMapping()
        {
            HasKey(pm => pm.Id);
            HasRequired(pm => pm.Item)
                .WithMany(i => i.PriceMutations)
                .HasForeignKey(pm => pm.ItemId);
            HasRequired(pm => pm.Group)
                .WithMany()
                .HasForeignKey(pm => pm.GroupId);
            Ignore(pm => pm.Errors);
        }
    }
}
