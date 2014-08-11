using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ContactMapping : EntityTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            HasKey(c => c.Id);
            HasMany(c => c.CoreIdentifications)
                .WithOptional(ci => ci.Contact)
                .HasForeignKey(ci => ci.ContactId);
            HasMany(c => c.BarringOrders)
                .WithRequired(bo => bo.Contact)
                .HasForeignKey(bo => bo.ContactId);
            HasMany(c => c.Barrings)
                .WithRequired(b => b.Contact)
                .HasForeignKey(c => c.ContactId);
            HasMany(c => c.PurchaseOrders)
                .WithRequired(po => po.Contact)
                .HasForeignKey(po => po.ContactId);
            HasMany(c => c.SalesOrders)
                .WithRequired(so => so.Contact)
                .HasForeignKey(so => so.ContactId);
            Ignore(c => c.Errors);
        }
    }
}