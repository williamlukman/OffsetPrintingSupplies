using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class RetailPurchaseInvoiceMapping : EntityTypeConfiguration<RetailPurchaseInvoice>
    {
        public RetailPurchaseInvoiceMapping()
        {
            HasKey(psi => psi.Id);
            HasMany(psi => psi.RetailPurchaseinvoiceDetails)
                .WithRequired(psid => psid.RetailPurchaseInvoice)
                .HasForeignKey(psid => psid.RetailPurchaseInvoiceId);
            HasRequired(ex => ex.Currency)
                .WithMany()
                .HasForeignKey(ex => ex.CurrencyId)
                .WillCascadeOnDelete(false);
            HasRequired(psi => psi.CashBank)
                .WithMany()
                .HasForeignKey(psi => psi.CashBankId)
                .WillCascadeOnDelete(false);
            HasRequired(psi => psi.Warehouse)
                .WithMany()
                .HasForeignKey(psi => psi.WarehouseId)
                .WillCascadeOnDelete(false);
            Ignore(psi => psi.Errors);
        }
    }
}
