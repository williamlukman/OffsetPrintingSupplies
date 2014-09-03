using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CoreIdentification
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int WarehouseId { get; set; } // Workshop Warehouse

        public Nullable<int> ContactId { get; set; }
        public bool IsInHouse { get; set; }
        public int Quantity { get; set; }
        public DateTime IdentifiedDate { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<CoreIdentificationDetail> CoreIdentificationDetails { get; set; }
    }
}