using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class BlanketMapping : EntityTypeConfiguration<Blanket>
    {
        public BlanketMapping()
        {
            ToTable("Blanket");
            HasRequired(b => b.Contact)
                .WithMany(c => c.Blankets)
                .HasForeignKey(b => b.ContactId)
                .WillCascadeOnDelete(false);
            HasRequired(b => b.Machine)
                .WithMany(m => m.Blankets)
                .HasForeignKey(b => b.MachineId);
            HasMany(b => b.BlanketOrderDetails)
                .WithRequired(bod => bod.Blanket)
                .HasForeignKey(bod => bod.BlanketId);
            HasRequired(b => b.RollBlanketItem)
                .WithMany()
                .HasForeignKey(b => b.RollBlanketItemId)
                .WillCascadeOnDelete(false);
        }
    }
}