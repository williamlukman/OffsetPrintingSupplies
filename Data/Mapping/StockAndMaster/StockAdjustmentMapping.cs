using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class StockAdjustmentMapping : EntityTypeConfiguration<StockAdjustment>
    {
        public StockAdjustmentMapping()
        {
            HasKey(sa => sa.Id);
            HasRequired(sa => sa.Warehouse)
                .WithMany()
                .HasForeignKey(sa => sa.WarehouseId)
                .WillCascadeOnDelete(false);
            HasMany(sa => sa.StockAdjustmentDetails)
                .WithRequired(sad => sad.StockAdjustment)
                .HasForeignKey(sad => sad.StockAdjustmentId);
            Ignore(sa => sa.Errors);
        }
    }
}
