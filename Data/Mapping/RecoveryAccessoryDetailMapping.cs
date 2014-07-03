using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RecoveryAccessoryDetailMapping : EntityTypeConfiguration<RecoveryAccessoryDetail>
    {
        public RecoveryAccessoryDetailMapping()
        {
            HasKey(rad => rad.Id);
            Ignore(rad => rad.Errors);
        }
    }
}