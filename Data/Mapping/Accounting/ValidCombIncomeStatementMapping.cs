using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ValidCombIncomeStatementMapping : EntityTypeConfiguration<ValidCombIncomeStatement>
    {
        public ValidCombIncomeStatementMapping()
        {
            HasKey(vc => vc.Id);
            HasRequired(vc => vc.Account)
                .WithMany()
                .HasForeignKey(vc => vc.AccountId)
                .WillCascadeOnDelete(false);
            HasRequired(vc => vc.Closing)
                .WithMany()
                .HasForeignKey(vc => vc.ClosingId)
                .WillCascadeOnDelete(false);
            Ignore(vc => vc.Errors);
        }
    }
}
