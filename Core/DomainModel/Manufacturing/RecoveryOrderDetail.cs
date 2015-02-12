using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class RecoveryOrderDetail
    {
        public int Id { get; set; }
        public int RecoveryOrderId { get; set; }
        public int CoreIdentificationDetailId { get; set; }
        public int RollerBuilderId { get; set; }
        public Nullable<int> CompoundUnderLayerId { get; set; }

        public decimal TotalCost { get; set; }
        public decimal AccessoriesCost { get; set; }
        public decimal CoreCost { get; set; }
        public decimal CompoundCost { get; set; }

        public decimal CompoundUsage { get; set; }
        public decimal CompoundUnderLayerUsage { get; set; }

        public string CoreTypeCase { get; set; }
        public bool IsDisassembled { get; set; }
        public bool IsStrippedAndGlued { get; set; }
        public bool IsWrapped { get; set; }
        public bool IsVulcanized { get; set; }
        public bool IsFacedOff { get; set; }
        public bool IsConventionalGrinded { get; set; }
        public bool IsCNCGrinded { get; set; }
        public bool IsPolishedAndQC { get; set; }
        public bool IsPackaged { get; set; }

        public bool IsRejected { get; set; }
        public Nullable<DateTime> RejectedDate { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<DateTime> FinishedDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual CoreIdentificationDetail CoreIdentificationDetail { get; set; }
        public virtual RecoveryOrder RecoveryOrder { get; set; }
        public virtual RollerBuilder RollerBuilder { get; set; }
        public Item CompoundUnderLayer { get; set; }
        public virtual ICollection<RecoveryAccessoryDetail> RecoveryAccessoryDetails { get; set; }
    }
}