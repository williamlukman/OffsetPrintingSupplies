using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceiptVoucherMapping : EntityTypeConfiguration<ReceiptVoucher>
    {
        public ReceiptVoucherMapping()
        {
            HasKey(rv => rv.Id);
            HasRequired(rv => rv.Contact)
                .WithMany()
                .HasForeignKey(rv => rv.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(ex => ex.Currency)
                .WithMany()
                .HasForeignKey(ex => ex.CurrencyId)
                .WillCascadeOnDelete(false);
            HasRequired(ex => ex.ExchangeRate)
                .WithMany()
                .HasForeignKey(ex => ex.ExchangeRateId)
                .WillCascadeOnDelete(false);
            HasMany(rv => rv.ReceiptVoucherDetails)
                .WithRequired(rvd => rvd.ReceiptVoucher)
                .HasForeignKey(rvd => rvd.ReceiptVoucherId);
            HasRequired(rv => rv.CashBank)
                .WithMany()
                .HasForeignKey(rv => rv.CashBankId)
                .WillCascadeOnDelete(false);
            Ignore(rv => rv.Errors);
        }
    }
}
