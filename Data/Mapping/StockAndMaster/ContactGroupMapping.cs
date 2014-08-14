using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class ContactGroupMapping : EntityTypeConfiguration<ContactGroup>
    {
        public ContactGroupMapping()
        {
            HasKey(cg => cg.Id);
            HasMany(cg => cg.Contacts)
                .WithRequired(c => c.ContactGroup)
                .HasForeignKey(c => c.ContactGroupId);
            Ignore(cg => cg.Errors);
        }
    }
}
