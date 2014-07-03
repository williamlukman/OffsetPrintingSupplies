using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class MachineMapping : EntityTypeConfiguration<Machine>
    {
        public MachineMapping()
        {
            HasKey(m => m.Id);
            Ignore(m => m.Errors);
        }
    }
}