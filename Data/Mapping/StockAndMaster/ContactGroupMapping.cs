using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ContactGroupMapping : EntityTypeConfiguration<ContactGroup>
    {
        public ContactGroupMapping()
        {
            HasKey(cg => cg.Id);
            HasMany(cg => cg.Contacts)
                .WithOptional(c => c.ContactGroup)
                .HasForeignKey(c => c.ContactGroupId);
            Ignore(cg => cg.Errors);
        }
    }
}