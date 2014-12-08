using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CustomerStockAdjustmentDetailMapping : EntityTypeConfiguration<CustomerStockAdjustmentDetail>
    {
        public CustomerStockAdjustmentDetailMapping()
        {
            HasKey(sad => sad.Id);
            HasRequired(sad => sad.Item)
                .WithMany()
                .HasForeignKey(sad => sad.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(sad => sad.CustomerStockAdjustment)
                .WithMany(sa => sa.CustomerStockAdjustmentDetails)
                .HasForeignKey(sad => sad.CustomerStockAdjustmentId);
            Ignore(sad => sad.Errors);
        }
    }
}
