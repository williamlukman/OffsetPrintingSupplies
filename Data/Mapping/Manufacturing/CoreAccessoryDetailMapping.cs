using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CoreAccessoryDetailMapping : EntityTypeConfiguration<CoreAccessoryDetail>
    {
        public CoreAccessoryDetailMapping()
        {
            HasKey(rad => rad.Id);
            HasRequired(rad => rad.CoreIdentificationDetail)
                .WithMany()
                .HasForeignKey(rad => rad.CoreIdentificationDetailId)
                .WillCascadeOnDelete(false);
            HasRequired(rad => rad.Item)
                .WithMany()
                .HasForeignKey(rad => rad.ItemId)
                .WillCascadeOnDelete(false);
            Ignore(rad => rad.Errors);
        }
    }
}