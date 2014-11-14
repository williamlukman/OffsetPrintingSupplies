using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PayableMapping : EntityTypeConfiguration<Payable>
    {
        public PayableMapping()
        {
            HasKey(p => p.Id);
            HasRequired(p => p.Contact)
                .WithMany()
                .HasForeignKey(p => p.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(ex => ex.Currency)
                 .WithMany()
                 .HasForeignKey(ex => ex.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasMany(p => p.PaymentVoucherDetails)
                .WithRequired(pvd => pvd.Payable)
                .HasForeignKey(pvd => pvd.PayableId);
            HasMany(p => p.PurchaseDownPaymentAllocationDetails)
                .WithRequired(pdpad => pdpad.Payable)
                .HasForeignKey(pdpad => pdpad.PayableId);
            HasMany(p => p.SalesDownPaymentAllocations)
                .WithRequired(sdpa => sdpa.Payable)
                .HasForeignKey(sdpa => sdpa.PayableId);
            HasMany(p => p.PurchaseAllowanceDetails)
                .WithRequired(pad => pad.Payable)
                .HasForeignKey(pad => pad.PayableId);
            Ignore(p => p.Errors);
        }
    }
}
