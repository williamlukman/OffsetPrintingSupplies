using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class DeliveryOrderMapping : EntityTypeConfiguration<DeliveryOrder>
    {
        public DeliveryOrderMapping()
        {
            HasKey(pr => pr.Id);
            HasRequired(pr => pr.Customer)
                .WithMany(c => c.DeliveryOrders)
                .WillCascadeOnDelete(false);
            HasOptional(pr => pr.DeliveryOrderDetails);
            Ignore(pr => pr.Errors);
        }
    }
}
