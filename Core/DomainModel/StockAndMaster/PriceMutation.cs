using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PriceMutation
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        //public int ContactGroupId { get; set; }
        public decimal Amount { get; set; }

        public bool IsActive { get; set; }
        public Nullable<DateTime> DeactivatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        //public virtual ContactGroup ContactGroup { get; set; }
    }
}
