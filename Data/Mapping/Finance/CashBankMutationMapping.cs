using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CashBankMutationMapping : EntityTypeConfiguration<CashBankMutation>
    {
        public CashBankMutationMapping()
        {
            HasKey(cbm => cbm.Id);
            Ignore(cbm => cbm.Errors);
        }
    }
}
