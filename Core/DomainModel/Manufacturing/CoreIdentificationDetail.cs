using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CoreIdentificationDetail
    {
        public int Id { get; set; }
        public string RollerNo { get; set; }

        public int CoreIdentificationId { get; set; }
        public int DetailId { get; set; }
        public int MaterialCase { get; set; }
        public int CoreBuilderId { get; set; }
        public int RollerTypeId { get; set; }

        public int MachineId { get; set; }
        public int RepairRequestCase { get; set; }
 
        public decimal RD { get; set; }
        public decimal CD { get; set; }
        public decimal RL { get; set; }
        public decimal WL { get; set; }
        public decimal TL { get; set; }

        public bool IsJobScheduled { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsRollerBuilt { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual CoreIdentification CoreIdentification { get; set; }
        public virtual CoreBuilder CoreBuilder { get; set; }
        public virtual RollerType RollerType { get; set; }
        public virtual Machine Machine { get; set; }
    }
}