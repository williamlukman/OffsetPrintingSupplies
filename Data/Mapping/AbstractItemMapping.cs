using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class AbstractItemMapping : EntityTypeConfiguration<AbstractItem>
    {
        public AbstractItemMapping()
        {
            ToTable("AbstractItem");
            HasKey(i => i.Id);
            HasRequired(i => i.ItemType)
                .WithMany(it => it.AbstractItems)
                .HasForeignKey(i => i.ItemTypeId);
            Ignore(i => i.Errors);
        }
    }
}
