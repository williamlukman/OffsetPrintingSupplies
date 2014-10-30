using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseDownPaymentAllocationDetailMapping : EntityTypeConfiguration<PurchaseDownPaymentAllocationDetail>
    {
        public PurchaseDownPaymentAllocationDetailMapping()
        {
            HasKey(pdpad => pdpad.Id);
            HasRequired(pdpad => pdpad.PurchaseDownPaymentAllocation)
                .WithMany(pdpa => pdpa.PurchaseDownPaymentAllocationDetails)
                .HasForeignKey(pdpad => pdpad.PurchaseDownPaymentAllocationId);
            HasRequired(pdpad => pdpad.Payable)
                .WithMany(p => p.PurchaseDownPaymentAllocationDetails)
                .HasForeignKey(pdpad => pdpad.PayableId);
            Ignore(pdpd => pdpd.Errors);
        }
    }
}
