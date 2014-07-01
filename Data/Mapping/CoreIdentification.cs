using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class CoreIdentificationMapping : EntityTypeConfiguration<CoreIdentification>
    {
        public CoreIdentificationMapping()
        {
            HasKey(ci => ci.Id);
            Ignore(ci => ci.Errors);
        }
    }
}