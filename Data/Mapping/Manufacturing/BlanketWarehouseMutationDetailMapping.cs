using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlanketWarehouseMutationDetailMapping : EntityTypeConfiguration<BlanketWarehouseMutationDetail>
    {
        public BlanketWarehouseMutationDetailMapping()
        {
            HasKey(rwmd => rwmd.Id);
            HasRequired(rwmd => rwmd.BlanketWarehouseMutation)
                .WithMany(rwm => rwm.BlanketWarehouseMutationDetails)
                .HasForeignKey(rwmd => rwmd.BlanketWarehouseMutationId);
            // supposed to be 1 to 0-1 relationship rather than 1 to many.
            // changing WithMany to WithOptional causes error in the EF
            HasRequired(rwmd => rwmd.BlanketOrderDetail)
                .WithMany()
                .HasForeignKey(rwmd => rwmd.BlanketOrderDetailId)
                .WillCascadeOnDelete(false);
            HasRequired(rwmd => rwmd.Item)
                .WithMany()
                .HasForeignKey(rwmd => rwmd.ItemId)
                .WillCascadeOnDelete(false);
            Ignore(rwmd => rwmd.Errors);
        }
    }
}
