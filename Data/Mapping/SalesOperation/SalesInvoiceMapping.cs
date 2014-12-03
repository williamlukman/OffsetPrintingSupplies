﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesInvoiceMapping : EntityTypeConfiguration<SalesInvoice>
    {
        public SalesInvoiceMapping()
        {
            HasKey(si => si.Id);
            HasRequired(si => si.DeliveryOrder)
                .WithMany()
                .HasForeignKey(si => si.DeliveryOrderId)
                .WillCascadeOnDelete(false);
            HasRequired(si => si.Currency)
                .WithMany()
                .HasForeignKey(si => si.CurrencyId)
                .WillCascadeOnDelete(false);
            HasOptional(si => si.ExchangeRate)
                .WithMany()
                .HasForeignKey(si => si.ExchangeRateId)
                .WillCascadeOnDelete(false);
            HasMany(si => si.SalesInvoiceDetails)
                .WithRequired(sid => sid.SalesInvoice)
                .HasForeignKey(sid => sid.SalesInvoiceId);
            Ignore(si => si.Errors);
        }
    }
}
