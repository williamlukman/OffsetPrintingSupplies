using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class RecoveryOrder
    {
        public int Id { get; set; }
        public int CoreIdentificationId { get; set; }
        public int WarehouseId { get; set; } // Workshop Warehouse

        public string Code { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityRejected { get; set; }
        public int QuantityFinal { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual CoreIdentification CoreIdentification { get; set; }
        public virtual ICollection<RecoveryOrderDetail> RecoveryOrderDetails { get; set; }
    }
}