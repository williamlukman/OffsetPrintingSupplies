using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class RollerWarehouseMutation
    {
        public int Id { get; set; }
        public int RecoveryOrderId { get; set; }
        public string Code { get; set; }

        public int WarehouseFromId { get; set; }
        public int WarehouseToId { get; set; }
        public DateTime MutationDate { get; set; }

        public int Quantity { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Warehouse WarehouseTo { get; set; }
        public virtual Warehouse WarehouseFrom { get; set; }
        public virtual RecoveryOrder RecoveryOrder { get; set; }
        public virtual ICollection<RollerWarehouseMutationDetail> RollerWarehouseMutationDetails { get; set; }
    }
}
