using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class StockMutationMapping : EntityTypeConfiguration<StockMutation>
    {
        public StockMutationMapping()
        {
            HasKey(sm => sm.Id);
            HasRequired(sm => sm.Item)
                .WithMany()
                .HasForeignKey(sm => sm.ItemId)
                .WillCascadeOnDelete(false);
            Ignore(sm => sm.Errors);
        }
    }
}
