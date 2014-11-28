using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseReceivalMapping : EntityTypeConfiguration<PurchaseReceival>
    {
        public PurchaseReceivalMapping()
        {
            HasKey(pr => pr.Id);
            HasMany(pr => pr.PurchaseReceivalDetails)
                .WithRequired(prd => prd.PurchaseReceival)
                .HasForeignKey(prd => prd.PurchaseReceivalId);
            HasOptional(ex => ex.ExchangeRate)
                .WithMany()
                .HasForeignKey(ex => ex.ExchangeRateId)
                .WillCascadeOnDelete(false);
            HasRequired(pr => pr.PurchaseOrder)
                .WithMany()
                .HasForeignKey(pr => pr.PurchaseOrderId)
                .WillCascadeOnDelete(false);
            HasRequired(pr => pr.Warehouse)
                .WithMany()
                .HasForeignKey(pr => pr.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(pr => pr.Errors);
        }
    }
}
