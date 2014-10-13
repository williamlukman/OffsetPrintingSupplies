using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesAllowanceMapping : EntityTypeConfiguration<SalesAllowance>
    {
        public SalesAllowanceMapping()
        {
            HasKey(sa => sa.Id);
            HasRequired(sa => sa.Contact)
                .WithMany()
                .HasForeignKey(sa => sa.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(sa => sa.SalesAllowanceDetails)
                .WithRequired(sad => sad.SalesAllowance)
                .HasForeignKey(sad => sad.SalesAllowanceId);
            HasRequired(sa => sa.CashBank)
                .WithMany()
                .HasForeignKey(sa => sa.CashBankId)
                .WillCascadeOnDelete(false);
            Ignore(sa => sa.Errors);
        }
    }
}
