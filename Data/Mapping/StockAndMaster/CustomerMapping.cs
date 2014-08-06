using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            HasKey(c => c.Id);
            HasMany(c => c.CoreIdentifications)
                .WithOptional(ci => ci.Customer)
                .HasForeignKey(ci => ci.CustomerId);
            HasMany(c => c.BarringOrders)
                .WithRequired(bo => bo.Customer)
                .HasForeignKey(bo => bo.CustomerId);
            HasMany(c => c.Barrings)
                .WithRequired(b => b.Customer)
                .HasForeignKey(c => c.CustomerId);

            HasOptional(c => c.PurchaseOrders);
            HasOptional(c => c.PurchaseOrderDetails);
            HasOptional(c => c.PurchaseReceivals);
            HasOptional(c => c.PurchaseReceivalDetails);
            HasOptional(c => c.SalesOrders);
            HasOptional(c => c.SalesOrderDetails);
            HasOptional(c => c.DeliveryOrders);
            HasOptional(c => c.DeliveryOrderDetails);

            Ignore(c => c.Errors);
        }
    }
}