using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class RepackingMapping : EntityTypeConfiguration<Repacking>
    {
        public RepackingMapping()
        {
            HasKey(r => r.Id);
            HasRequired(r => r.BlendingRecipe)
                .WithMany()
                .HasForeignKey(r => r.BlendingRecipeId)
                .WillCascadeOnDelete(false);
            HasRequired(r => r.Warehouse)
                .WithMany()
                .HasForeignKey(r => r.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(r => r.Errors);
        }
    }
}