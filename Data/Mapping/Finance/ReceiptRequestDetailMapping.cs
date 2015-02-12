using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceiptRequestDetailMapping : EntityTypeConfiguration<ReceiptRequestDetail>
    {
        public ReceiptRequestDetailMapping()
        {
            HasKey(rrd => rrd.Id);
            HasRequired(rrd => rrd.Account)
                .WithMany()
                .HasForeignKey(rrd => rrd.AccountId)
                .WillCascadeOnDelete(false);
            HasRequired(rrd => rrd.ReceiptRequest)
                .WithMany(rr => rr.ReceiptRequestDetails)
                .HasForeignKey(rrd => rrd.ReceiptRequestId);
            Ignore(rrd => rrd.Errors);
        }
    }
}
