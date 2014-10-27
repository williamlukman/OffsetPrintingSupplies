using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesOrderMapping : EntityTypeConfiguration<SalesOrder>
    {
        public SalesOrderMapping()
        {
            HasKey(so => so.Id);
            HasRequired(so => so.Contact)
                .WithMany(c => c.SalesOrders)
                .HasForeignKey(so => so.ContactId);
            HasMany(so => so.SalesOrderDetails)
                .WithRequired(sod => sod.SalesOrder)
                .HasForeignKey(sod => sod.SalesOrderId);
            HasRequired(so => so.SalesQuotation)
                .WithMany()
                .HasForeignKey(so => so.SalesQuotationId)
                .WillCascadeOnDelete(false);
            Ignore(so => so.Errors);
        }
    }
}
