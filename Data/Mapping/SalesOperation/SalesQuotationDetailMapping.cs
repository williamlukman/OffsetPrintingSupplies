using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesQuotationDetailMapping : EntityTypeConfiguration<SalesQuotationDetail>
    {
        public SalesQuotationDetailMapping()
        {
            HasKey(sqd => sqd.Id);
            HasRequired(sqd => sqd.SalesQuotation)
                .WithMany(sq => sq.SalesQuotationDetails)
                .HasForeignKey(sqd => sqd.SalesQuotationId);
            Ignore(sqd => sqd.Errors);
        }
    }
}
