using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseDownPaymentDetailMapping : EntityTypeConfiguration<PurchaseDownPaymentDetail>
    {
        public PurchaseDownPaymentDetailMapping()
        {
            HasKey(pdpd => pdpd.Id);
            HasRequired(pdpd => pdpd.PurchaseDownPayment)
                .WithMany(pdp => pdp.PurchaseDownPaymentDetails)
                .HasForeignKey(pdpd => pdpd.PurchaseDownPaymentId);
            HasRequired(pdpd => pdpd.Payable)
                .WithMany(pdp => pdp.PurchaseDownPaymentDetails)
                .HasForeignKey(pdpd => pdpd.PayableId);
            Ignore(pdpd => pdpd.Errors);
        }
    }
}
