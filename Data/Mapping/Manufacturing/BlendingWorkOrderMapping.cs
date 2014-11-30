using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlendingWorkOrderMapping : EntityTypeConfiguration<BlendingWorkOrder>
    {
        public BlendingWorkOrderMapping()
        {
            HasKey(bo => bo.Id);
            HasRequired(bo => bo.BlendingRecipe)
                .WithMany()
                .HasForeignKey(bo => bo.BlendingRecipeId)
                .WillCascadeOnDelete(false);
            HasRequired(bo => bo.Warehouse)
                .WithMany()
                .HasForeignKey(bo => bo.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(bo => bo.Errors);
        }
    }
}