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
            HasOptional(b => b.Adhesive)
                .WithMany()
                .HasForeignKey(b => b.AdhesiveId)
                .WillCascadeOnDelete(false);
            HasOptional(b => b.Adhesive2)
                .WithMany()
                .HasForeignKey(b => b.Adhesive2Id)
                .WillCascadeOnDelete(false);
            HasOptional(b => b.LeftBarItem)
                .WithMany()
                .HasForeignKey(b => b.LeftBarItemId)
                .WillCascadeOnDelete(false);
            HasOptional(b => b.RightBarItem)
                .WithMany()
                .HasForeignKey(b => b.RightBarItemId)
                .WillCascadeOnDelete(false);
        }
    }
}