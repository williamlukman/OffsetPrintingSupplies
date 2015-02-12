using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ReceiptRequest
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int CurrencyId { get; set; }
        public int AccountReceivableId { get; set; }
         
        public decimal Amount { get; set; }
        public decimal ExchangeRateAmount { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<int> ExchangeRateId { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual ICollection<ReceiptRequestDetail> ReceiptRequestDetails { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Account AccountReceivable { get; set; }
        public Dictionary<String, String> Errors { get; set; }
        public virtual ExchangeRate ExchangeRate { get; set; }

    }
}
