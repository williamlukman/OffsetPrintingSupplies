﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceivableMapping : EntityTypeConfiguration<Receivable>
    {
        public ReceivableMapping()
        {
            HasKey(r => r.Id);
            HasRequired(r => r.Contact)
                .WithMany()
                .HasForeignKey(r => r.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(r => r.Currency)
                 .WithMany()
                 .HasForeignKey(r => r.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasMany(r => r.ReceiptVoucherDetails)
                .WithRequired(rvd => rvd.Receivable)
                .HasForeignKey(rvd => rvd.ReceivableId);
            HasMany(r => r.SalesDownPaymentAllocationDetails)
                .WithRequired(sdpad => sdpad.Receivable)
                .HasForeignKey(sdpad => sdpad.ReceivableId);
            HasMany(r => r.PurchaseDownPaymentAllocations)
                .WithRequired(pdpa => pdpa.Receivable)
                .HasForeignKey(pdpa => pdpa.ReceivableId);
            HasMany(r => r.SalesAllowanceDetails)
                .WithRequired(sad => sad.Receivable)
                .HasForeignKey(sad => sad.ReceivableId);
            Ignore(r => r.Errors);
        }
    }
}
