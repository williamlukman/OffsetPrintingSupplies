using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int OrderType { get; set; }
        public string OrderCode { get; set; }
        //public string CustomerPORef { get; set; }
        public int ContactId { get; set; }
        public DateTime SalesDate { get; set; }
        public string NomorSurat { get; set; }
        public int CurrencyId { get; set; }
        public Nullable<int> EmployeeId { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeliveryCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Employee Employee { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}