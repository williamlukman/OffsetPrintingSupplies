using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BankAdministrationMapping : EntityTypeConfiguration<BankAdministration>
    {
        public BankAdministrationMapping()
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
            HasMany(pr => pr.BankAdministrationDetails)
                .WithRequired(prd => prd.BankAdministration)
                .HasForeignKey(prd => prd.BankAdministrationId);
            Ignore(cba => cba.Errors);
        }
    }
}
