using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string SkuRollerUsedCore { get; set; }
        public string SkuRollerNewCore { get; set; }
        public int RollerUsedCoreItemId { get; set; }
        public int RollerNewCoreItemId { get; set; }
        public int UoMId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RD { get; set; }
        public decimal CD { get; set; }
        public decimal RL { get; set; }
        public decimal WL { get; set; }
        public decimal TL { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual UoM UoM { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual CoreBuilder CoreBuilder { get; set; }
        public virtual RollerType RollerType { get; set; }
    }
}
