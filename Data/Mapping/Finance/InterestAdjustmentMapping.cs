using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class InterestAdjustmentMapping : EntityTypeConfiguration<InterestAdjustment>
    {
        public InterestAdjustmentMapping()
        {
            HasKey(cba => cba.Id);
            HasRequired(cba => cba.CashBank)
                .WithMany()
                .HasForeignKey(cba => cba.CashBankId)
                .WillCascadeOnDelete(false);
            HasOptional(cba => cba.ExchangeRate)
                .WithMany()
                .HasForeignKey(cba => cba.ExchangeRateId)
                .WillCascadeOnDelete(false);
            Ignore(cba => cba.Errors);
        }
    }
}
