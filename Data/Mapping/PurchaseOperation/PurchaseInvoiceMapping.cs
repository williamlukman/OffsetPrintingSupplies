using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseInvoiceMapping : EntityTypeConfiguration<PurchaseInvoice>
    {
        public PurchaseInvoiceMapping()
        {
            HasKey(pi => pi.Id);
            HasRequired(pi => pi.Currency)
                 .WithMany()
                 .HasForeignKey(pi => pi.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasOptional(pi => pi.ExchangeRate)
                .WithMany()
                .HasForeignKey(pi => pi.ExchangeRateId)
                .WillCascadeOnDelete(false);
            HasRequired(pi => pi.PurchaseReceival)
                .WithMany()
                .HasForeignKey(pi => pi.PurchaseReceivalId)
                .WillCascadeOnDelete(false);
            HasMany(pi => pi.PurchaseInvoiceDetails)
                .WithRequired(pid => pid.PurchaseInvoice)
                .HasForeignKey(pid => pid.PurchaseInvoiceId);
            Ignore(pi => pi.Errors);
        }
    }
}
