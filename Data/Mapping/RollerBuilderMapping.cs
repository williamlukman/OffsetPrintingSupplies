using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RollerBuilderMapping : EntityTypeConfiguration<RollerBuilder>
    {
        public RollerBuilderMapping()
        {
            HasKey(rb => rb.Id);
            Ignore(rb => rb.Errors);
        }
    }
}