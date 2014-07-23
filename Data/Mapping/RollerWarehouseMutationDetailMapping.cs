using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class RollerWarehouseMutationDetailMapping : EntityTypeConfiguration<RollerWarehouseMutationDetail>
    {
        public RollerWarehouseMutationDetailMapping()
        {
            HasKey(rwmd => rwmd.Id);
            HasRequired(rwmd => rwmd.RollerWarehouseMutation)
                .WithMany(rwm => rwm.RollerWarehouseMutationDetails)
                .HasForeignKey(rwmd => rwmd.RollerWarehouseMutationId);
            HasRequired(rwmd => rwmd.CoreIdentificationDetail)
                .WithMany()
                .HasForeignKey(rwm => rwm.CoreIdentificationDetailId)
                .WillCascadeOnDelete(false);
            Ignore(rwmd => rwmd.Errors);
        }
    }
}
