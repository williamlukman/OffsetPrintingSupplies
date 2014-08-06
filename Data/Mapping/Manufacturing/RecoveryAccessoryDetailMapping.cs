using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RecoveryAccessoryDetailMapping : EntityTypeConfiguration<RecoveryAccessoryDetail>
    {
        public RecoveryAccessoryDetailMapping()
        {
            HasKey(rad => rad.Id);
            HasRequired(rad => rad.RecoveryOrderDetail)
                .WithMany(rod => rod.RecoveryAccessoryDetails)
                .HasForeignKey(rad => rad.RecoveryOrderDetailId);
            HasRequired(rad => rad.Item)
                .WithMany(i => i.RecoveryAccessoryDetails)
                .HasForeignKey(rad => rad.ItemId);
            Ignore(rad => rad.Errors);
        }
    }
}