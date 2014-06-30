using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class RollerRecoveryAccessoryDetail
    {
        public int Id { get; set; }
        public int RollerRecoveryDetailId { get; set; }
        public int AccessoryId {get; set;}
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual RollerRecoveryDetail RollerRecoveryDetail { get; set; }
    }
}
