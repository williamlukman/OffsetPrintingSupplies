﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class PaymentVoucher
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CashBankId { get; set; }
        public string Code { get; set; }
        public string NoBukti { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsGBCH { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public bool IsReconciled { get; set; }
        public Nullable<DateTime> ReconciliationDate { get; set; }
        public decimal RateToIDR { get; set; }


        public decimal TotalAmount { get; set; }
        public decimal BiayaBank { get; set; }
        public decimal Pembulatan { get; set; }
        public int StatusPembulatan { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual CashBank CashBank { get; set; }
        public virtual ICollection<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
