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
            Ignore(cid => cid.Errors);
        }
    }
}