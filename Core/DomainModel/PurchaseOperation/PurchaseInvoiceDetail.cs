﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PurchaseInvoiceDetail
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int PurchaseReceivalDetailId { get; set; }
        public string Code { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
        public virtual PurchaseReceivalDetail PurchaseReceivalDetail { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}