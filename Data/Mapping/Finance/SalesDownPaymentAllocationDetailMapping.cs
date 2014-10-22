using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesDownPaymentAllocationDetailMapping : EntityTypeConfiguration<SalesDownPaymentAllocationDetail>
    {
        public SalesDownPaymentAllocationDetailMapping()
        {
            HasKey(sdpad => sdpad.Id);
            HasRequired(sdpad => sdpad.SalesDownPaymentAllocation)
                .WithMany(sdpa => sdpa.SalesDownPaymentAllocationDetails)
                .HasForeignKey(sdpad => sdpad.SalesDownPaymentAllocationId);
            HasRequired(sdpad => sdpad.Receivable)
                .WithMany(p => p.SalesDownPaymentAllocationDetails)
                .HasForeignKey(sdpad => sdpad.ReceivableId);
            HasRequired(sdpad => sdpad.ReceiptVoucherDetail)
                .WithMany()
                .HasForeignKey(sdpad => sdpad.ReceiptVoucherDetailId)
                .WillCascadeOnDelete(false);
            Ignore(sdpd => sdpd.Errors);
        }
    }
}
