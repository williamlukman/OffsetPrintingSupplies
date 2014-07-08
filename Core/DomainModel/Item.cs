using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class Item : AbstractItem
    {
        public string Description { get; set; }
        public virtual ICollection<RecoveryAccessoryDetail> RecoveryAccessoryDetails { get; set; }
    }
}
