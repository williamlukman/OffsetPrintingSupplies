using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CustomerStockMutationMapping : EntityTypeConfiguration<CustomerStockMutation>
    {
        public CustomerStockMutationMapping()
        {
            HasKey(csm => csm.Id);
            HasOptional(csm => csm.CustomerItem)
                .WithMany()
                .HasForeignKey(csm => csm.CustomerItemId)
                .WillCascadeOnDelete(false);
            HasOptional(csm => csm.Contact)
                .WithMany()
                .HasForeignKey(csm => csm.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(csm => csm.Item)
                .WithMany()
                .HasForeignKey(csm => csm.ItemId)
                .WillCascadeOnDelete(false);
            Ignore(csm => csm.Errors);
        }
    }
}
