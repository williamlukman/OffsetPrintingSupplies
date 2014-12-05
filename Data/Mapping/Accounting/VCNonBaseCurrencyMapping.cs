using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class VCNonBaseCurrencyMapping : EntityTypeConfiguration<VCNonBaseCurrency>
    {
        public VCNonBaseCurrencyMapping()
        {
            HasKey(vc => vc.Id);
            HasRequired(vc => vc.ValidComb)
                .WithMany()
                .HasForeignKey(vc => vc.ValidCombId);
            Ignore(vc => vc.Errors);
        }
    }
}
