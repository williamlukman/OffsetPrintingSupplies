using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesDownPayment
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public Nullable<int> ReceivableId { get; set; }
        public Nullable<int> PayableId { get; set; }
        public string Code { get; set; }
        public DateTime DownPaymentDate { get; set; }
        public Nullable<DateTime> DueDate { get; set; }

        public int CurrencyId { get; set; }
        public decimal ExchangeRateAmount { get; set; }
        public Nullable<int> ExchangeRateId { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Receivable Receivable { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Payable Payable { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
