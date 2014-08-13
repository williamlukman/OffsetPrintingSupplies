using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class GroupItemPriceMapping : EntityTypeConfiguration<GroupItemPrice>
    {
        public GroupItemPriceMapping()
        {
            HasKey(gip => gip.Id);
            HasRequired(gip => gip.Item)
                .WithMany()
                .HasForeignKey(gip => gip.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(gip => gip.Group)
                .WithMany()
                .HasForeignKey(gip => gip.GroupId)
                .WillCascadeOnDelete(false);
            HasRequired(gip => gip.PriceMutation)
                .WithMany()
                .HasForeignKey(gip => gip.PriceMutationId)
                .WillCascadeOnDelete(false);
            Ignore(gip => gip.Errors);
        }
    }
}
