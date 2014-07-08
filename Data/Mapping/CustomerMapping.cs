using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Ignore(c => c.Errors);
        }
    }
}