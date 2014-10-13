using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseAllowanceDetailMapping : EntityTypeConfiguration<PurchaseAllowanceDetail>
    {
        public PurchaseAllowanceDetailMapping()
        {
            HasKey(pad => pad.Id);
            HasRequired(pad => pad.PurchaseAllowance)
                .WithMany(pa => pa.PurchaseAllowanceDetails)
                .HasForeignKey(pad => pad.PurchaseAllowanceId);
            HasRequired(pad => pad.Payable)
                .WithMany(pa => pa.PurchaseAllowanceDetails)
                .HasForeignKey(pad => pad.PayableId);
            Ignore(pad => pad.Errors);
        }
    }
}
