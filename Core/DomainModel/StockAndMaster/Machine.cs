﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Machine
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual ICollection<Blanket> Blankets { get; set; }
        public virtual ICollection<RollerBuilder> RollerBuilders { get; set; }
    }
}
