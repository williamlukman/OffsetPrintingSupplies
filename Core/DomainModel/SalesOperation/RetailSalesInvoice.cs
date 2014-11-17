﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class RetailSalesInvoice
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime SalesDate { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public bool IsGroupPricing { get; set; }
        public int ContactId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public bool IsConfirmed { get; set; }

        public bool IsGBCH { get; set; }
        public string GBCH_No { get; set; }
        public Nullable<DateTime> GBCH_DueDate { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public decimal ExchangeRateAmount { get; set; }
        public bool IsBank { get; set; }
        public bool IsFullPayment { get; set; }
        public bool IsPaid { get; set; }
        public int CashBankId { get; set; }
        public int WarehouseId { get; set; }

        public decimal Total { get; set; }
        public decimal CoGS { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<RetailSalesInvoiceDetail> RetailSalesinvoiceDetails { get; set; }
    }
}
