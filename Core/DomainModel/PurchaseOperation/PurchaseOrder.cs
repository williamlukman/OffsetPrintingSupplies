using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ContactId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string NomorSurat { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsReceivalCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual Contact Contact {get; set;}
        public Dictionary<String, String> Errors { get; set; }

    }
}