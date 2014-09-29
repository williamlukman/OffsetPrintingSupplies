using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class VirtualOrderDetailMapping : EntityTypeConfiguration<VirtualOrderDetail>
    {
        public VirtualOrderDetailMapping()
        {
            HasKey(vod => vod.Id);
            HasRequired(vod => vod.VirtualOrder)
                .WithMany(vo => vo.VirtualOrderDetails)
                .HasForeignKey(vod => vod.VirtualOrderId);
            Ignore(vod => vod.Errors);
        }
    }
}
