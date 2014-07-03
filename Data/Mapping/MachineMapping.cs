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
            HasMany(m => m.RollerBuilders)
                .WithRequired(rb => rb.Machine)
                .HasForeignKey(rb => rb.MachineId);
            Ignore(m => m.Errors);
        }
    }
}