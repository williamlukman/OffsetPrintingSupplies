using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ItemType
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLegacy { get; set; } // Core, Roller, Blanket are legacy items
        public Nullable<int> AccountId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual Account Account { get; set; }
    }
}
