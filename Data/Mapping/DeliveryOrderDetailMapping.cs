using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class DeliveryOrderDetailMapping : EntityTypeConfiguration<DeliveryOrderDetail>
    {
        public DeliveryOrderDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(dod => dod.Customer)
                .WithMany(c => c.DeliveryOrderDetails)
                .WillCascadeOnDelete(false);
            HasRequired(dod => dod.DeliveryOrder)
                .WithMany(d => d.DeliveryOrderDetails)
                .HasForeignKey(dod => dod.DeliveryOrderId);
            HasOptional(dod => dod.SalesOrderDetail)
                .WithOptionalDependent(sod => sod.DeliveryOrderDetail);
            Ignore(pod => pod.Errors);
        }
    }
}
