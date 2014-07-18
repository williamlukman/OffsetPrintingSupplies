using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class BarringOrderDetail
    {
        public int Id { get; set; }
        public int BarringOrderId { get; set; }
        public int BarringId { get; set; }

        public bool IsBarRequired { get; set; }
        public bool HasLeftBar { get; set; }
        public bool HasRightBar { get; set; }
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

        public bool IsJobScheduled { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<DateTime> FinishDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual BarringOrder BarringOrder { get; set; }
        public virtual Barring Barring { get; set; }
    }
}