using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class SalesOrderDetailMapping : EntityTypeConfiguration<SalesOrderDetail>
    {
        public SalesOrderDetailMapping()
        {
            HasKey(sod => sod.Id);
            HasRequired(sod => sod.Customer)
                .WithMany(c => c.SalesOrderDetails)
                .HasForeignKey(sod => sod.CustomerId);
            HasRequired(sod => sod.SalesOrder)
                .WithMany(po => po.SalesOrderDetails)
                .HasForeignKey(sod => sod.SalesOrderId);
            Ignore(sod => sod.Errors);
        }
    }
}
