using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class TemporaryDeliveryOrderClearanceDetailMapping : EntityTypeConfiguration<TemporaryDeliveryOrderClearanceDetail>
    {
        public TemporaryDeliveryOrderClearanceDetailMapping()
        {
            HasKey(tdocd => tdocd.Id);
            HasRequired(tdocd => tdocd.TemporaryDeliveryOrderClearance)
                .WithMany(tdoc => tdoc.TemporaryDeliveryOrderClearanceDetails)
                .HasForeignKey(tdocd => tdocd.TemporaryDeliveryOrderClearanceId);
            HasOptional(tdocd => tdocd.TemporaryDeliveryOrderDetail)
                .WithMany()
                .HasForeignKey(tdocd => tdocd.TemporaryDeliveryOrderDetailId)
                .WillCascadeOnDelete(false);
            Ignore(tdocd => tdocd.Errors);
        }
    }
}
