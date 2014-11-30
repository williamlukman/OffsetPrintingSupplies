using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class VirtualOrderMapping : EntityTypeConfiguration<VirtualOrder>
    {
        public VirtualOrderMapping()
        {
            HasKey(vo => vo.Id);
            HasRequired(ex => ex.Currency)
                 .WithMany()
                 .HasForeignKey(ex => ex.CurrencyId)
                 .WillCascadeOnDelete(false);
            HasRequired(vo => vo.Contact)
                .WithMany(v => v.VirtualOrders)
                .HasForeignKey(vo => vo.ContactId);
            HasMany(vo => vo.VirtualOrderDetails)
                .WithRequired(vod => vod.VirtualOrder)
                .HasForeignKey(vod => vod.VirtualOrderId);
            Ignore(vo => vo.Errors);
        }
    }
}
