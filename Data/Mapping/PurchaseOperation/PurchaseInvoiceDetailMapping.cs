using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseInvoiceDetailMapping : EntityTypeConfiguration<PurchaseInvoiceDetail>
    {
        public PurchaseInvoiceDetailMapping()
        {
            HasKey(pi => pi.Id);
            HasRequired(pid => pid.PurchaseInvoice)
                .WithMany(pi => pi.PurchaseInvoiceDetails)
                .HasForeignKey(pid => pid.PurchaseInvoiceId);
            HasRequired(pid => pid.PurchaseReceivalDetail)
                .WithMany()
                .HasForeignKey(pid => pid.PurchaseReceivalDetailId)
                .WillCascadeOnDelete(false);
            Ignore(i => i.Errors);
        }
    }
}
