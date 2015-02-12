﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ContactDetail
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }

        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public Contact Contact { get; set; }
    }
}