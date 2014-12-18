using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesInvoiceMigrationMapping : EntityTypeConfiguration<SalesInvoiceMigration>
    {
        public SalesInvoiceMigrationMapping()
        {
            HasKey(sim => sim.Id);
            HasRequired(sim => sim.Currency)
                .WithMany()
                .HasForeignKey(sim => sim.CurrencyId)
                .WillCascadeOnDelete(false);
            HasRequired(sim => sim.Contact)
                .WithMany()
                .HasForeignKey(sim => sim.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(sim => sim.Errors);
        }
    }
}
