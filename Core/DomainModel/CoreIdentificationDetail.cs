using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class CoreIdentificationDetail
    {
        public int Id { get; set; }
        public int CoreIdentificationId { get; set; }
        public int DetailId { get; set; }
        public int MaterialCase { get; set; }
        public int CoreBuilderId { get; set; }
        public int RollerTypeId { get; set; }

        public int MachineId { get; set; }
        public decimal RD { get; set; }
        public decimal CD { get; set; }
        public decimal RL { get; set; }
        public decimal WL { get; set; }
        public decimal TL { get; set; }

        public bool IsJobScheduled { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<DateTime> FinishDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual CoreIdentification CoreIdentification { get; set; }
        public virtual CoreBuilder CoreBuilder { get; set; }
    }
}