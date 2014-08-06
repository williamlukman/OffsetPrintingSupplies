using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PaymentVoucherMapping : EntityTypeConfiguration<PaymentVoucher>
    {
        public PaymentVoucherMapping()
        {
            HasKey(pv => pv.Id);
            HasRequired(pv => pv.Customer)
                .WithMany()
                .HasForeignKey(pv => pv.CustomerId)
                .WillCascadeOnDelete(false);
            HasMany(pv => pv.PaymentVoucherDetails)
                .WithRequired(pvd => pvd.PaymentVoucher)
                .HasForeignKey(pvd => pvd.PaymentVoucherId);
            Ignore(pv => pv.Errors);
        }
    }
}
