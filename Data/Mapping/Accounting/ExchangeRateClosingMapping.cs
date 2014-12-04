using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ExchangeRateClosingMapping : EntityTypeConfiguration<ExchangeRateClosing>
    {
        public ExchangeRateClosingMapping()
        {
            HasKey(vc => vc.Id);
            HasRequired(pvd => pvd.Closing)
               .WithMany(pv => pv.ExchangeRateClosings)
               .HasForeignKey(pvd => pvd.ClosingId);
            HasRequired(vc => vc.Currency)
                .WithMany()
                .HasForeignKey(vc => vc.CurrencyId)
                .WillCascadeOnDelete(false);
            Ignore(vc => vc.Errors);
        }
    }
}
