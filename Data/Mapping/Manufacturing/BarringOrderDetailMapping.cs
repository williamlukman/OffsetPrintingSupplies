using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BarringOrderDetailMapping : EntityTypeConfiguration<BarringOrderDetail>
    {
        public BarringOrderDetailMapping()
        {
            HasKey(bod => bod.Id);
            HasRequired(bod => bod.BarringOrder)
                .WithMany(bo => bo.BarringOrderDetails)
                .HasForeignKey(bod => bod.BarringOrderId);
            HasRequired(bod => bod.Barring)
                .WithMany(b => b.BarringOrderDetails)
                .HasForeignKey(bod => bod.BarringId);
            Ignore(bod => bod.Errors);
        }
    }
}