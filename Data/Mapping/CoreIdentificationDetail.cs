using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class CoreIdentificationDetailMapping : EntityTypeConfiguration<CoreIdentificationDetail>
    {
        public CoreIdentificationDetailMapping()
        {
            HasKey(cid => cid.Id);
            Ignore(cid => cid.Errors);
        }
    }
}