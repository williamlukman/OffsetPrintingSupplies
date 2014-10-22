using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesDownPaymentAllocationMapping : EntityTypeConfiguration<SalesDownPaymentAllocation>
    {
        public SalesDownPaymentAllocationMapping()
        {
            HasKey(sdpa => sdpa.Id);
            HasRequired(sdpa => sdpa.Contact)
                .WithMany()
                .HasForeignKey(sdpa => sdpa.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(sdpa => sdpa.SalesDownPaymentAllocationDetails)
                .WithRequired(sdpad => sdpad.SalesDownPaymentAllocation)
                .HasForeignKey(sdpad => sdpad.SalesDownPaymentAllocationId);
            HasRequired(sdpa => sdpa.SalesDownPayment)
                .WithMany()
                .HasForeignKey(sdpa => sdpa.SalesDownPaymentId)
                .WillCascadeOnDelete(false);
            Ignore(sdpa => sdpa.Errors);
        }
    }
}
