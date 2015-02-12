﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ReceiptRequestDetail
    {
        public int Id { get; set; }
        public int ReceiptRequestId { get; set; }
        public int AccountId { get; set; }
        public string Code { get; set; }
        
        public int Status { get; set; }
        public decimal Amount { get; set; }

        public bool IsLegacy { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual ReceiptRequest ReceiptRequest { get; set; }
        public virtual Account Account { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}
