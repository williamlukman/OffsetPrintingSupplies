using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PaymentRequestMapping : EntityTypeConfiguration<PaymentRequest>
    {
        public PaymentRequestMapping()
        {
            HasKey(pr => pr.Id);
            HasRequired(pr => pr.Contact)
                .WithMany()
                .HasForeignKey(pr => pr.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(pr => pr.Errors);
        }
    }
}
