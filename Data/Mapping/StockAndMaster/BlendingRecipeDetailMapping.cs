using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlendingRecipeDetailMapping : EntityTypeConfiguration<BlendingRecipeDetail>
    {
        public BlendingRecipeDetailMapping()
        {
            ToTable("BlendingRecipeDetail");
            HasKey(b => b.Id);
            HasRequired(b => b.Item)
                .WithMany()
                .HasForeignKey(b => b.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(b => b.BlendingRecipe)
                .WithMany(b => b.BlendingRecipeDetails)
                .HasForeignKey(b => b.BlendingRecipeId)
                .WillCascadeOnDelete(false);
            Ignore(b => b.Errors);
        }
    }
}