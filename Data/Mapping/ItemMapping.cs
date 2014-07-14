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
            ToTable("Item");
            HasKey(i => i.Id);
            HasRequired(i => i.ItemType)
                .WithMany(it => it.Items)
                .HasForeignKey(i => i.ItemTypeId);
            HasMany(i => i.RecoveryAccessoryDetails)
                .WithRequired(rad => rad.Item)
                .HasForeignKey(rad => rad.ItemId);
            Ignore(i => i.Errors);
        }
    }
}
