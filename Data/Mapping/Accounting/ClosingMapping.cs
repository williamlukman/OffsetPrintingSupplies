using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ClosingMapping : EntityTypeConfiguration<Closing>
    {
        public ClosingMapping()
        {
            HasKey(c => c.Id);
            HasMany(c => c.ValidCombs)
                .WithRequired(v => v.Closing)
                .HasForeignKey(v => v.ClosingId);
            HasMany(c => c.ExchangeRateClosings)
              .WithRequired(v => v.Closing)
              .HasForeignKey(v => v.ClosingId)
              .WillCascadeOnDelete(true);
            Ignore(c => c.Errors);
        }
    }
}
