using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesOrderDetailMapping : EntityTypeConfiguration<SalesOrderDetail>
    {
        public SalesOrderDetailMapping()
        {
            HasKey(sod => sod.Id);
            HasRequired(sod => sod.SalesOrder)
                .WithMany(po => po.SalesOrderDetails)
                .HasForeignKey(sod => sod.SalesOrderId);
            HasRequired(sod => sod.Item)
               .WithMany()
               .HasForeignKey(sod => sod.ItemId)
               .WillCascadeOnDelete(false);
            Ignore(sod => sod.Errors);
        }
    }
}
