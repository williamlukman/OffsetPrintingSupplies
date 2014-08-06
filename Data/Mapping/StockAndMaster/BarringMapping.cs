using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BarringMapping : EntityTypeConfiguration<Barring>
    {
        public BarringMapping()
        {
            ToTable("Barring");
            HasRequired(b => b.Customer)
                .WithMany(c => c.Barrings)
                .HasForeignKey(b => b.CustomerId)
                .WillCascadeOnDelete(false);
            HasRequired(b => b.Machine)
                .WithMany(m => m.Barrings)
                .HasForeignKey(b => b.MachineId);
            HasMany(b => b.BarringOrderDetails)
                .WithRequired(bod => bod.Barring)
                .HasForeignKey(bod => bod.BarringId);
        }
    }
}