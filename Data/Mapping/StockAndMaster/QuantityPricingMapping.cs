using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class QuantityPricingMapping : EntityTypeConfiguration<QuantityPricing>
    {
        public QuantityPricingMapping()
        {
            HasKey(qp => qp.Id);
            HasRequired(qp => qp.ItemType)
                .WithMany()
                .HasForeignKey(qp => qp.ItemTypeId)
                .WillCascadeOnDelete(false);
            Ignore(qp => qp.Errors);
        }
    }
}
