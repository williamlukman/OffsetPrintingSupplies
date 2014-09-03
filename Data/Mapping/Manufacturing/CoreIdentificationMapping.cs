using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CoreIdentificationMapping : EntityTypeConfiguration<CoreIdentification>
    {
        public CoreIdentificationMapping()
        {
            HasKey(ci => ci.Id);
            HasOptional(ci => ci.Contact)
                .WithMany(c => c.CoreIdentifications)
                .HasForeignKey(ci => ci.ContactId);
            HasRequired(ci => ci.Warehouse)
                .WithMany()
                .HasForeignKey(ci => ci.WarehouseId)
                .WillCascadeOnDelete(false);
            HasMany(ci => ci.CoreIdentificationDetails)
                .WithRequired(cid => cid.CoreIdentification)
                .HasForeignKey(cid => cid.CoreIdentificationId);
            Ignore(ci => ci.Errors);
        }
    }
}