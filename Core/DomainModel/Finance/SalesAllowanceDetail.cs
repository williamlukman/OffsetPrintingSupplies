﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class SalesAllowanceDetail
    {
        public int Id { get; set; }
        public int SalesAllowanceId { get; set; }
        public int ReceivableId { get; set; }
        public string Code { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual SalesAllowance SalesAllowance { get; set; }
        public virtual Receivable Receivable { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}