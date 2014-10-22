using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesDownPaymentMapping : EntityTypeConfiguration<SalesDownPayment>
    {
        public SalesDownPaymentMapping()
        {
            HasKey(sdp => sdp.Id);
            HasRequired(sdp => sdp.ReceiptVoucher)
                .WithMany()
                .HasForeignKey(sdp => sdp.ReceiptVoucherId)
                .WillCascadeOnDelete(false);
            HasRequired(sdp => sdp.CashBank)
                .WithMany()
                .HasForeignKey(sdp => sdp.CashBankId)
                .WillCascadeOnDelete(false);
            HasRequired(sdp => sdp.Contact)
                .WithMany()
                .HasForeignKey(sdp => sdp.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(sdp => sdp.Errors);
        }
    }
}
