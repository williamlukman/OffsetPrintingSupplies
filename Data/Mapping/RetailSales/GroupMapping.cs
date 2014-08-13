using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class GroupMapping : EntityTypeConfiguration<Group>
    {
        public GroupMapping()
        {
            HasKey(g => g.Id);
            HasMany(g => g.Contacts)
                .WithRequired(c => c.Group)
                .HasForeignKey(c => c.GroupId);
            Ignore(g => g.Errors);
        }
    }
}
