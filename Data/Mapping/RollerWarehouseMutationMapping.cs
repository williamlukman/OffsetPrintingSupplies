using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RollerWarehouseMutationMapping : EntityTypeConfiguration<RollerWarehouseMutation>
    {
        public RollerWarehouseMutationMapping()
        {
            HasKey(rwm => rwm.Id);
            HasMany(rwm => rwm.RollerWarehouseMutationDetails)
                .WithRequired(rwmd => rwmd.RollerWarehouseMutation)
                .HasForeignKey(rwmd => rwmd.RollerWarehouseMutationId);
            //HasRequired(rwm => rwm.CoreIdentification)
            //    .WithMany(ci => ci.RollerWarehouseMutations)
            //    .HasForeignKey(rwm => rwm.CoreIdentificationId);
            Ignore(rwm => rwm.Errors);
        }
    }
}
