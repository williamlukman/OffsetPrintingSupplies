using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class GLNonBaseCurrencyMapping : EntityTypeConfiguration<GLNonBaseCurrency>
    {
        public GLNonBaseCurrencyMapping()
        {
            HasKey(glj => glj.Id);
            HasRequired(cba => cba.GeneralLedgerJournal)
             .WithMany()
             .HasForeignKey(cba => cba.GeneralLedgerJournalId)
             .WillCascadeOnDelete(true);
            Ignore(glj => glj.Errors);
        }
    }
}
