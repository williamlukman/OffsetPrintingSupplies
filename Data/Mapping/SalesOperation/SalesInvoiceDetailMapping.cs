using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesInvoiceDetailMapping : EntityTypeConfiguration<SalesInvoiceDetail>
    {
        public SalesInvoiceDetailMapping()
        {
            HasKey(sid => sid.Id);
            HasRequired(sid => sid.SalesInvoice)
                .WithMany(si => si.SalesInvoiceDetails)
                .HasForeignKey(sid => sid.SalesInvoiceId);
            HasRequired(sid => sid.DeliveryOrderDetail)
                .WithMany()
                .HasForeignKey(sid => sid.DeliveryOrderDetailId)
                .WillCascadeOnDelete(false);
            Ignore(sid => sid.Errors);
        }
    }
}
