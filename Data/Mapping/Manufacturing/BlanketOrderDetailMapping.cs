using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlanketOrderDetailMapping : EntityTypeConfiguration<BlanketOrderDetail>
    {
        public BlanketOrderDetailMapping()
        {
            HasKey(bod => bod.Id);
            HasRequired(bod => bod.BlanketOrder)
                .WithMany(bo => bo.BlanketOrderDetails)
                .HasForeignKey(bod => bod.BlanketOrderId);
            HasRequired(bod => bod.Blanket)
                .WithMany(b => b.BlanketOrderDetails)
                .HasForeignKey(bod => bod.BlanketId);
            Ignore(bod => bod.Errors);
        }
    }
}