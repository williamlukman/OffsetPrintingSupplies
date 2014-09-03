using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CoreIdentificationDetailMapping : EntityTypeConfiguration<CoreIdentificationDetail>
    {
        public CoreIdentificationDetailMapping()
        {
            HasKey(cid => cid.Id);
            HasRequired(cid => cid.CoreIdentification)
                .WithMany(ci => ci.CoreIdentificationDetails)
                .HasForeignKey(cid => cid.CoreIdentificationId);
            HasRequired(cid => cid.CoreBuilder)
                .WithMany(cb => cb.CoreIdentificationDetails)
                .HasForeignKey(cid => cid.CoreBuilderId);
            HasRequired(cid => cid.RollerType)
                .WithMany()
                .HasForeignKey(cid => cid.RollerTypeId)
                .WillCascadeOnDelete(false);
            HasRequired(cid => cid.Machine)
                .WithMany()
                .HasForeignKey(cid => cid.MachineId)
                .WillCascadeOnDelete(false);
            Ignore(cid => cid.Errors);
        }
    }
}