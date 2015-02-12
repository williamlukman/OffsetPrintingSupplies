using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseInvoiceMigrationMapping : EntityTypeConfiguration<PurchaseInvoiceMigration>
    {
        public PurchaseInvoiceMigrationMapping()
        {
            HasKey(pim => pim.Id);
            HasRequired(pim => pim.Currency)
                 .WithMany()
                 .HasForeignKey(pim => pim.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasRequired(pim => pim.Contact)
                .WithMany()
                .HasForeignKey(pim => pim.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(pim => pim.Errors);
        }
    }
}
