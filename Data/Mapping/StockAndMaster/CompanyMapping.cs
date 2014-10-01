﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CompanyMapping : EntityTypeConfiguration<Company>
    {
        public CompanyMapping()
        {
            HasKey(c => c.Id);
            Ignore(c => c.Errors);
        }
    }
}