using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CashBankMapping : EntityTypeConfiguration<CashBank>
    {

        public CashBankMapping()
        {
            HasKey(cb => cb.Id);
            HasMany(cb => cb.CashMutations)
                .WithRequired(cm => cm.CashBank)
                .HasForeignKey(cm => cm.CashBankId);
            Ignore(cb => cb.Errors);
        }
    }
}
