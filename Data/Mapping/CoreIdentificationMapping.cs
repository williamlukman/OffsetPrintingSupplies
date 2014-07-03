using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class CoreIdentificationMapping : EntityTypeConfiguration<CoreIdentification>
    {
        public CoreIdentificationMapping()
        {
            HasKey(ci => ci.Id);
            HasRequired(ci => ci.Customer)
                .WithMany(c => c.CoreIdentifications)
                .HasForeignKey(ci => ci.CustomerId);
            HasMany(ci => ci.CoreIdentificationDetails)
                .WithRequired(cid => cid.CoreIdentification)
                .HasForeignKey(cid => cid.CoreIdentificationId);
            Ignore(ci => ci.Errors);
        }
    }
}