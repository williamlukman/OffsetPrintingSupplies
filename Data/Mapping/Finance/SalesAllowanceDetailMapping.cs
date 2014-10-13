using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalesAllowanceDetailMapping : EntityTypeConfiguration<SalesAllowanceDetail>
    {
        public SalesAllowanceDetailMapping()
        {
            HasKey(sad => sad.Id);
            HasRequired(sad => sad.SalesAllowance)
                .WithMany(pv => pv.SalesAllowanceDetails)
                .HasForeignKey(sad => sad.SalesAllowanceId);
            HasRequired(sad => sad.Receivable)
                .WithMany(r => r.SalesAllowanceDetails)
                .HasForeignKey(sad => sad.ReceivableId);
            Ignore(sad => sad.Errors);
        }
    }
}
