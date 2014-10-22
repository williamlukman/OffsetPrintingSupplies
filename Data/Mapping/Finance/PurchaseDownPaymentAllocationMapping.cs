using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseDownPaymentAllocationMapping : EntityTypeConfiguration<PurchaseDownPaymentAllocation>
    {
        public PurchaseDownPaymentAllocationMapping()
        {
            HasKey(pdpa => pdpa.Id);
            HasRequired(pdpa => pdpa.Contact)
                .WithMany()
                .HasForeignKey(pdpa => pdpa.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(pdpa => pdpa.PurchaseDownPaymentAllocationDetails)
                .WithRequired(pdpad => pdpad.PurchaseDownPaymentAllocation)
                .HasForeignKey(pdpad => pdpad.PurchaseDownPaymentAllocationId);
            HasRequired(pdpa => pdpa.PurchaseDownPayment)
                .WithMany()
                .HasForeignKey(pdpa => pdpa.PurchaseDownPaymentId)
                .WillCascadeOnDelete(false);
            Ignore(pdpa => pdpa.Errors);
        }
    }
}
