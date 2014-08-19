using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class CashSalesReturnDetailMapping : EntityTypeConfiguration<CashSalesReturnDetail>
    {
        public CashSalesReturnDetailMapping()
        {
            HasKey(csrd => csrd.Id);
            HasRequired(csrd => csrd.CashSalesReturn)
                .WithMany(csr => csr.CashSalesReturnDetails)
                .HasForeignKey(csrd => csrd.CashSalesReturnId)
                .WillCascadeOnDelete(true); // Need to be True if CashBank table is deleted first (ie. when CashBank table deleted leading to CashSalesInvoice and CashSalesReturn deletion also)
            HasRequired(csrd => csrd.CashSalesInvoiceDetail) // Optional ?
                .WithMany(csid => csid.CashSalesReturnDetails)
                .HasForeignKey(csrd => csrd.CashSalesInvoiceDetailId);
            /*HasRequired(csrd => csrd.PriceMutation)
                .WithMany()
                .HasForeignKey(csrd => csrd.PriceMutationId)
                .WillCascadeOnDelete(false);*/
            Ignore(csrd => csrd.Errors);
        }
    }
}
