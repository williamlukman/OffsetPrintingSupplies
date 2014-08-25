﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WarehouseMutationDetailMapping : EntityTypeConfiguration<WarehouseMutationDetail>
    {
        public WarehouseMutationDetailMapping()
        {
            HasKey(wmod => wmod.Id);
            HasRequired(wmod => wmod.WarehouseMutation)
                .WithMany(wmo => wmo.WarehouseMutationDetails)
                .HasForeignKey(wmod => wmod.WarehouseMutationId);
            Ignore(wmod => wmod.Errors);
        }
    }
}
