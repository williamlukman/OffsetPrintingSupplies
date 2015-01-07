using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceiptRequestMapping : EntityTypeConfiguration<ReceiptRequest>
    {
        public ReceiptRequestMapping()
        {
            HasKey(rr => rr.Id);
            HasRequired(rr => rr.Contact)
                .WithMany()
                .HasForeignKey(rr => rr.ContactId)
                .WillCascadeOnDelete(false);
            HasOptional(rr => rr.ExchangeRate)
               .WithMany()
               .HasForeignKey(rr => rr.ExchangeRateId)
               .WillCascadeOnDelete(false);
            HasRequired(rr => rr.AccountReceivable)
                .WithMany()
                .HasForeignKey(rr => rr.AccountReceivableId)
                .WillCascadeOnDelete(false);
            HasMany(rr => rr.ReceiptRequestDetails)
                .WithRequired(rrd => rrd.ReceiptRequest)
                .HasForeignKey(rrd => rrd.ReceiptRequestId);
            Ignore(rr => rr.Errors);
        }
    }
}
