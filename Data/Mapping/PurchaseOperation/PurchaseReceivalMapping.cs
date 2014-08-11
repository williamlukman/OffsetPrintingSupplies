using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseReceivalMapping : EntityTypeConfiguration<PurchaseReceival>
    {
        public PurchaseReceivalMapping()
        {
            HasKey(pr => pr.Id);
            HasMany(pr => pr.PurchaseReceivalDetails)
                .WithRequired(prd => prd.PurchaseReceival)
                .HasForeignKey(prd => prd.PurchaseReceivalId);
            Ignore(pr => pr.Errors);
        }
    }
}
