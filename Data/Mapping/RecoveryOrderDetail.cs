using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RecoveryOrderDetailMapping : EntityTypeConfiguration<RecoveryOrderDetail>
    {
        public RecoveryOrderDetailMapping()
        {
            HasKey(rod => rod.Id);
            Ignore(rod => rod.Errors);
        }
    }
}