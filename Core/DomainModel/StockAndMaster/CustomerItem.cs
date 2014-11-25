using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CustomerItem
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public int Virtual { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Item Item { get; set; }
    }
}
