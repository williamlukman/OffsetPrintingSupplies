using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BankAdministrationDetailMapping : EntityTypeConfiguration<BankAdministrationDetail>
    {
        public BankAdministrationDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(prd => prd.Account)
                .WithMany()
                .HasForeignKey(prd => prd.AccountId)
                .WillCascadeOnDelete(false);
            HasRequired(prd => prd.BankAdministration)
                .WithMany(pr => pr.BankAdministrationDetails)
                .HasForeignKey(prd => prd.BankAdministrationId);
            Ignore(prd => prd.Errors);
        }
    }
}
