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
            HasOptional(ci => ci.Customer)
                .WithMany(c => c.CoreIdentifications)
                .HasForeignKey(ci => ci.CustomerId);
            HasMany(ci => ci.CoreIdentificationDetails)
                .WithRequired(cid => cid.CoreIdentification)
                .HasForeignKey(cid => cid.CoreIdentificationId);
            //HasMany(ci => ci.RollerWarehouseMutations)
            //    .WithRequired(rwm => rwm.CoreIdentification)
            //    .HasForeignKey(rwm => rwm.CoreIdentificationId);
            Ignore(ci => ci.Errors);
        }
    }
}