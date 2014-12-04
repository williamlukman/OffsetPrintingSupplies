using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class VirtualOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ContactId { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public string NomorSurat { get; set; }
        public int CurrencyId { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeliveryCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<VirtualOrderDetail> VirtualOrderDetails { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Contact Contact {get; set;}
        public Dictionary<String, String> Errors { get; set; }
    }
}