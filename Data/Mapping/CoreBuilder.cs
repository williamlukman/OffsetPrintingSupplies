using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class CoreBuilderMapping : EntityTypeConfiguration<CoreBuilder>
    {
        public CoreBuilderMapping()
        {
            HasKey(cb => cb.Id);
            Ignore(cb => cb.Errors);
        }
    }
}