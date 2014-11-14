using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CurrencyMapping : EntityTypeConfiguration<Currency>
    {
        public CurrencyMapping()
        {
            HasKey(ex => ex.Id);
            Ignore(ex => ex.Errors);
        }
    }
}
