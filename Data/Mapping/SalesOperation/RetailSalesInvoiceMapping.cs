using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class RetailSalesInvoiceMapping : EntityTypeConfiguration<RetailSalesInvoice>
    {
        public RetailSalesInvoiceMapping()
        {
            HasKey(rsi => rsi.Id);
            HasMany(rsi => rsi.RetailSalesinvoiceDetails)
                .WithRequired(rsid => rsid.RetailSalesInvoice)
                .HasForeignKey(rsid => rsid.RetailSalesInvoiceId);
            HasRequired(rsi => rsi.CashBank)
                .WithMany()
                .HasForeignKey(rsi => rsi.CashBankId)
                .WillCascadeOnDelete(true); // Need to be True if CashBank Table is deleted before RetailSalesInvoice table deleted
            HasRequired(rsi => rsi.Warehouse)
                .WithMany()
                .HasForeignKey(rsi => rsi.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(rsi => rsi.Errors);
        }
    }
}
