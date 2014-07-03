using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RecoveryOrderMapping : EntityTypeConfiguration<RecoveryOrder>
    {
        public RecoveryOrderMapping()
        {
            HasKey(ro => ro.Id);
            Ignore(ro => ro.Errors);
        }
    }
}