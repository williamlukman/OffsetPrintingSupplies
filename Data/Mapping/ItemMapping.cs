using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class ItemMapping : EntityTypeConfiguration<Item>
    {
        public ItemMapping()
        {
            HasKey(i => i.Id);
            HasRequired(i => i.ItemType)
                .WithMany(it => it.Items)
                .HasForeignKey(i => i.ItemTypeId);
            Ignore(i => i.Errors);
        }
    }
}