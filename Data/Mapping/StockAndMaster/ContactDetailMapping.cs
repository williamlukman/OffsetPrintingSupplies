using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ContactDetailMapping : EntityTypeConfiguration<ContactDetail>
    {
        public ContactDetailMapping()
        {
            HasKey(cd => cd.Id);
            HasRequired(cd => cd.Contact)
                .WithMany(c => c.ContactDetails)
                .HasForeignKey(cd => cd.ContactId);
            Ignore(cd => cd.Errors);
        }
    }
}