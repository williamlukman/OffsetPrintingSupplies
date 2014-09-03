using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class BlanketOrderDetail
    {
        public int Id { get; set; }
        // TODO
        // public string Code { get; set; }
        public int BlanketOrderId { get; set; }
        public int BlanketId { get; set; }

        public bool IsCut { get; set; }
        public bool IsSideSealed { get; set; }
        public bool IsBarPrepared { get; set; }
        public bool IsAdhesiveTapeApplied { get; set; }
        public bool IsBarMounted { get; set; }
        public bool IsBarHeatPressed { get; set; }
        public bool IsBarPullOffTested { get; set; }
        public bool IsQCAndMarked { get; set; }
        public bool IsPackaged { get; set; }
        public bool IsRejected { get; set; }
        public Nullable<DateTime> RejectedDate { get; set; }

        public bool IsJobScheduled { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<DateTime> FinishedDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual BlanketOrder BlanketOrder { get; set; }
        public virtual Blanket Blanket { get; set; }
    }
}