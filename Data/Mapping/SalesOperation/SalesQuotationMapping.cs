using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesQuotationMapping : EntityTypeConfiguration<SalesQuotation>
    {
        public SalesQuotationMapping()
        {
            HasKey(sq => sq.Id);
            HasRequired(sq => sq.Contact)
                .WithMany(c => c.SalesQuotations)
                .HasForeignKey(sq => sq.ContactId);
            HasMany(sq => sq.SalesQuotationDetails)
                .WithRequired(sqd => sqd.SalesQuotation)
                .HasForeignKey(sqd => sqd.SalesQuotationId);
            Ignore(sq => sq.Errors);
        }
    }
}
