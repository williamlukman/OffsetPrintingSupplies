using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class RollerBuilder
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int RollerTypeId { get; set; }
        public int CompoundId { get; set; }
        public int CoreBuilderId { get; set; }
        public string BaseSku { get; set; }
        public string SkuUsedRoller { get; set; }
        public string SkuNewRoller { get; set; }
        public int UsedRollerItemId { get; set; }
        public int NewRollerItemId { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public int RD { get; set; }
        public int CD { get; set; }
        public int RL { get; set; }
        public int WL { get; set; }
        public int TL { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual CoreBuilder CoreBuilder { get; set; }
        public virtual RollerType RollerType { get; set; }
    }
}
