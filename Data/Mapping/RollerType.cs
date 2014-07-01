using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RollerTypeMapping : EntityTypeConfiguration<RollerType>
    {
        public RollerTypeMapping()
        {
            HasKey(rt => rt.Id);
            Ignore(rt => rt.Errors);
        }
    }
}