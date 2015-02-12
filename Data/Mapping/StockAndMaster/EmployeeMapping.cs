using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeMapping : EntityTypeConfiguration<Employee>
    {
        public EmployeeMapping()
        {
            HasKey(e => e.Id);
            HasMany(e => e.SalesOrders)
                .WithOptional(so => so.Employee)
                .HasForeignKey(so => so.EmployeeId);
            Ignore(e => e.Errors);
        }
    }
}