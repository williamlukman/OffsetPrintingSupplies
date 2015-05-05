using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ClosingReportMapping : EntityTypeConfiguration<ClosingReport>
    {
        public ClosingReportMapping()
        {
            HasKey(glj => glj.Id);
            HasRequired(cba => cba.Closing)
                .WithMany()
                .HasForeignKey(cba => cba.ClosingId)
                .WillCascadeOnDelete(true);
            HasRequired(i => i.Currency)
                .WithMany()
                .HasForeignKey(i => i.CurrencyId);
            HasRequired(cba => cba.Account)
               .WithMany()
               .HasForeignKey(cba => cba.AccountId);
            HasRequired(cba => cba.Parent)
               .WithMany()
               .HasForeignKey(cba => cba.AccountParentId);
            HasOptional(cba => cba.Contact)
               .WithMany()
               .HasForeignKey(cba => cba.ContactId);
            Ignore(glj => glj.Errors);
        }
    }
}
