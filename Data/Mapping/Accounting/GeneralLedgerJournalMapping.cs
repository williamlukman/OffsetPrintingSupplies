﻿using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class GeneralLedgerJournalMapping : EntityTypeConfiguration<GeneralLedgerJournal>
    {
        public GeneralLedgerJournalMapping()
        {
            HasKey(glj => glj.Id);
            Ignore(glj => glj.Errors);
        }
    }
}
