using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SubTypeMapping : EntityTypeConfiguration<SubType>
    {
        public SubTypeMapping()
        {
            HasKey(st => st.Id);
            HasRequired(st => st.ItemType)
                .WithMany()
                .HasForeignKey(i => i.ItemTypeId)
                .WillCascadeOnDelete(false);
            HasMany(st => st.Items)
                .WithOptional(i => i.SubType)
                .HasForeignKey(i => i.SubTypeId);
            Ignore(st => st.Errors);
        }
    }
}