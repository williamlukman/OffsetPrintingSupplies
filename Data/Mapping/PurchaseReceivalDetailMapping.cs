using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class PurchaseReceivalDetailMapping : EntityTypeConfiguration<PurchaseReceivalDetail>
    {
        public PurchaseReceivalDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(prd => prd.Customer)
                .WithMany(c => c.PurchaseReceivalDetails)
                .HasForeignKey(prd => prd.CustomerId);
            HasRequired(prd => prd.PurchaseReceival)
                .WithMany(pr => pr.PurchaseReceivalDetails)
                .HasForeignKey(prd => prd.PurchaseReceivalId);
            HasOptional(prd => prd.PurchaseOrderDetail)
                .WithOptionalDependent(pod => pod.PurchaseReceivalDetail);
            Ignore(pod => pod.Errors);
        }
    }
}
