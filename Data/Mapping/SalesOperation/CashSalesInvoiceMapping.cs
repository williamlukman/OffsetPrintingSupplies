using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class CashSalesInvoiceMapping : EntityTypeConfiguration<CashSalesInvoice>
    {
        public CashSalesInvoiceMapping()
        {
            HasKey(csi => csi.Id);
            HasMany(csi => csi.CashSalesinvoiceDetails)
                .WithRequired(csid => csid.CashSalesInvoice)
                .HasForeignKey(csid => csid.CashSalesInvoiceId);
            HasRequired(csi => csi.CashBank)
                .WithMany()
                .HasForeignKey(csi => csi.CashBankId)
                .WillCascadeOnDelete(true); // Need to be True if CashBank Table is deleted before RetailSalesInvoice table deleted
            HasRequired(csi => csi.Warehouse)
                .WithMany()
                .HasForeignKey(csi => csi.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(csi => csi.Errors);
        }
    }
}
