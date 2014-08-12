using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel.RetailSales;

namespace Data.Mapping.RetailSales
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
