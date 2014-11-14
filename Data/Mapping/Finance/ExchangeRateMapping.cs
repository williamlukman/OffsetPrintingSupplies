using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ExchangeRateMapping : EntityTypeConfiguration<ExchangeRate>
    {
        public ExchangeRateMapping()
        {
            HasKey(ex => ex.Id);
            HasRequired(ex => ex.Currency)
                .WithMany()
                .HasForeignKey(ex => ex.CurrencyId)
                .WillCascadeOnDelete(false);
            Ignore(ex => ex.Errors);
        }
    }
}
