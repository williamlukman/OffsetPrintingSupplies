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
            HasRequired(sdp => sdp.Receivable)
                .WithMany()
                .HasForeignKey(sdp => sdp.ReceivableId)
                .WillCascadeOnDelete(false);
            HasRequired(sdp => sdp.Contact)
                .WithMany()
                .HasForeignKey(sdp => sdp.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(sdp => sdp.Errors);
        }
    }
}
