using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlendingRecipeMapping : EntityTypeConfiguration<BlendingRecipe>
    {
        public BlendingRecipeMapping()
        {
            ToTable("BlendingRecipe");
            HasKey(b => b.Id);
            HasRequired(b => b.TargetItem)
                .WithMany()
                .HasForeignKey(b => b.TargetItemId)
                .WillCascadeOnDelete(false);
            HasMany(b => b.BlendingRecipeDetails)
                .WithRequired(bd => bd.BlendingRecipe)
                .HasForeignKey(bd => bd.BlendingRecipeId);
            Ignore(b => b.Errors);
        }
    }
}