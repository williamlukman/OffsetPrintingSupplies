using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class PriceMutationMapping : EntityTypeConfiguration<PriceMutation>
    {
        public PriceMutationMapping()
        {
            HasKey(pm => pm.Id);
            HasRequired(pm => pm.Item)
                .WithMany(i => i.PriceMutations)
                .HasForeignKey(pm => pm.ItemId);
            /*HasRequired(pm => pm.ContactGroup)
                .WithMany()
                .HasForeignKey(pm => pm.ContactGroupId);*/
            Ignore(pm => pm.Errors);
        }
    }
}
