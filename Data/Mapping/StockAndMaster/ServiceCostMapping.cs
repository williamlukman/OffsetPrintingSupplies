using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ServiceCostMapping : EntityTypeConfiguration<ServiceCost>
    {
        public ServiceCostMapping()
        {
            HasKey(sc => sc.Id);
            HasRequired(sc => sc.Item)
                .WithMany()
                .HasForeignKey(sc => sc.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(sc => sc.RollerBuilder)
                .WithMany()
                .HasForeignKey(sc => sc.RollerBuilderId)
                .WillCascadeOnDelete(false);
            Ignore(sc => sc.Errors);
        }
    }
}