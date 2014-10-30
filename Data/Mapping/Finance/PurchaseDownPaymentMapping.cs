using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseDownPaymentMapping : EntityTypeConfiguration<PurchaseDownPayment>
    {
        public PurchaseDownPaymentMapping()
        {
            HasKey(pdp => pdp.Id);
            HasRequired(pdp => pdp.Payable)
                .WithMany()
                .HasForeignKey(pdp => pdp.PayableId)
                .WillCascadeOnDelete(false);
            HasRequired(pdp => pdp.CashBank)
                .WithMany()
                .HasForeignKey(pdp => pdp.CashBankId)
                .WillCascadeOnDelete(false);
            HasRequired(pdp => pdp.Contact)
                .WithMany()
                .HasForeignKey(pdp => pdp.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(pdp => pdp.Errors);
        }
    }
}
