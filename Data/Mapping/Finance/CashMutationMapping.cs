using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CashMutationMapping : EntityTypeConfiguration<CashMutation>
    {

        public CashMutationMapping()
        {
            HasKey(cm => cm.Id);
            HasRequired(cm => cm.CashBank)
                .WithMany(cb => cb.CashMutations)
                .HasForeignKey(cm => cm.CashBankId);
            Ignore(cm => cm.Errors);
        }
    }
}
