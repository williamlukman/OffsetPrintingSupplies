using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class CashSalesReturnMapping : EntityTypeConfiguration<CashSalesReturn>
    {
        public CashSalesReturnMapping()
        {
            HasKey(csr => csr.Id);
            HasMany(csr => csr.CashSalesReturnDetails)
                .WithRequired(csrd => csrd.CashSalesReturn)
                .HasForeignKey(csrd => csrd.CashSalesReturnId);
            HasRequired(csr => csr.CashSalesInvoice) // Optional ?
                .WithMany(csi => csi.CashSalesReturns)
                .HasForeignKey(csr => csr.CashSalesInvoiceId);
            HasRequired(csr => csr.CashBank)
                .WithMany()
                .HasForeignKey(csr => csr.CashBankId)
                .WillCascadeOnDelete(true); // Need to be True if CashBank Table is deleted before RetailSalesInvoice table deleted
            Ignore(csr => csr.Errors);
        }
    }
}
