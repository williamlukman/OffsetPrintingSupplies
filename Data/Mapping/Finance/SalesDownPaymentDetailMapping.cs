using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesDownPaymentDetailMapping : EntityTypeConfiguration<SalesDownPaymentDetail>
    {
        public SalesDownPaymentDetailMapping()
        {
            HasKey(sdpd => sdpd.Id);
            HasRequired(sdpd => sdpd.SalesDownPayment)
                .WithMany(pv => pv.SalesDownPaymentDetails)
                .HasForeignKey(sdpd => sdpd.SalesDownPaymentId);
            HasRequired(sdpd => sdpd.Receivable)
                .WithMany(r => r.SalesDownPaymentDetails)
                .HasForeignKey(sdpd => sdpd.ReceivableId);
            Ignore(sdpd => sdpd.Errors);
        }
    }
}
