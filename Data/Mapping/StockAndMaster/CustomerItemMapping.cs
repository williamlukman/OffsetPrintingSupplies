using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CustomerItemMapping : EntityTypeConfiguration<CustomerItem>
    {
        public CustomerItemMapping()
        {
            HasKey(ci => ci.Id);
            HasRequired(ci => ci.Item)
                .WithMany()
                .HasForeignKey(ci => ci.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(ci => ci.Contact)
                .WithMany()
                .HasForeignKey(ci => ci.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(ci => ci.Errors);
        }
    }
}
