using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class TemporaryDeliveryOrderClearanceMapping : EntityTypeConfiguration<TemporaryDeliveryOrderClearance>
    {
        public TemporaryDeliveryOrderClearanceMapping()
        {
            HasKey(tdoc => tdoc.Id);
            HasMany(tdoc => tdoc.TemporaryDeliveryOrderClearanceDetails)
                .WithRequired(tdocd => tdocd.TemporaryDeliveryOrderClearance)
                .HasForeignKey(tdocd => tdocd.TemporaryDeliveryOrderClearanceId);
            HasOptional(tdoc => tdoc.TemporaryDeliveryOrder)
                .WithMany()
                .HasForeignKey(tdoc => tdoc.TemporaryDeliveryOrderId)
                .WillCascadeOnDelete(false);
            Ignore(tdoc => tdoc.Errors);
        }
    }
}
