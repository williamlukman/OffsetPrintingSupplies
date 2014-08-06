using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class RollerWarehouseMutationDetail
    {
        public int Id { get; set; }
        public int RollerWarehouseMutationId { get; set; }
        public int CoreIdentificationDetailId { get; set; }

        public int ItemId { get; set; }

        public bool IsFinished { get; set; }
        public Nullable<DateTime> FinishedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual RollerWarehouseMutation RollerWarehouseMutation { get; set; }
        public virtual CoreIdentificationDetail CoreIdentificationDetail { get; set; }
    }
}
