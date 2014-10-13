using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PurchaseAllowanceMapping : EntityTypeConfiguration<PurchaseAllowance>
    {
        public PurchaseAllowanceMapping()
        {
            HasKey(pa => pa.Id);
            HasRequired(pa => pa.Contact)
                .WithMany()
                .HasForeignKey(pa => pa.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(pa => pa.PurchaseAllowanceDetails)
                .WithRequired(pad => pad.PurchaseAllowance)
                .HasForeignKey(pad => pad.PurchaseAllowanceId);
            HasRequired(pa => pa.CashBank)
                .WithMany()
                .HasForeignKey(pa => pa.CashBankId)
                .WillCascadeOnDelete(false);
            Ignore(pa => pa.Errors);
        }
    }
}
