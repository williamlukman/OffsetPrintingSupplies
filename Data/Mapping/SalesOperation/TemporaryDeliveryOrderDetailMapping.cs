using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class TemporaryDeliveryOrderDetailMapping : EntityTypeConfiguration<TemporaryDeliveryOrderDetail>
    {
        public TemporaryDeliveryOrderDetailMapping()
        {
            HasKey(tdod => tdod.Id);
            HasRequired(tdod => tdod.TemporaryDeliveryOrder)
                .WithMany(td => td.TemporaryDeliveryOrderDetails)
                .HasForeignKey(tdod => tdod.TemporaryDeliveryOrderId);
            HasOptional(tdod => tdod.SalesOrderDetail)
                .WithMany()
                .HasForeignKey(tdod => tdod.SalesOrderDetailId)
                .WillCascadeOnDelete(false);
            HasOptional(tdod => tdod.VirtualOrderDetail)
                .WithMany()
                .HasForeignKey(tdod => tdod.VirtualOrderDetailId)
                .WillCascadeOnDelete(false);
            Ignore(tdod => tdod.Errors);
        }
    }
}
