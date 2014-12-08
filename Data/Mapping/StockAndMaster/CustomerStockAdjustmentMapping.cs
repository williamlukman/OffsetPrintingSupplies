using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CustomerStockAdjustmentMapping : EntityTypeConfiguration<CustomerStockAdjustment>
    {
        public CustomerStockAdjustmentMapping()
        {
            HasKey(sa => sa.Id);
            HasRequired(sa => sa.Warehouse)
                .WithMany()
                .HasForeignKey(sa => sa.WarehouseId)
                .WillCascadeOnDelete(false);
            HasRequired(sa => sa.Contact)
                .WithMany()
                .HasForeignKey(sa => sa.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(sa => sa.CustomerStockAdjustmentDetails)
                .WithRequired(sad => sad.CustomerStockAdjustment)
                .HasForeignKey(sad => sad.CustomerStockAdjustmentId);
            Ignore(sa => sa.Errors);
        }
    }
}
